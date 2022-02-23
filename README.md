# Gio's Gameplay Framework for Unity

Games change **a lot** during development, but code *isn't always easy to change*. Games also need *a lot of testing*, but code isn't always *easy to test*. Teams can sometimes change during development, and code *isn't always easy to understand*, especially for newcomers. With these 3 issues in mind, I've developed this *Gameplay Framework*. It's been applied successfully in a commercial project, and the team has adapted to it quite well. Since Unity doesn't have a standard gameplay framework, I hope this helps other games get built more *easily* and *responsibly*.

## Table of contents
1. [Callbacks](#callbacks)
1. [Rules](#rules)
1. [Systems](#systems)
1. [Managers](#managers)
1. [Entities](#entities)
1. [Components](#components)
1. [Abstraction](#abstraction)
1. [Examples](#examples)

## Installation:

### OpenUPM
[![openupm](https://img.shields.io/npm/v/com.middlemast.gameplayframework?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.middlemast.gameplayframework/)

### [UPM - Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
1. Install the core dependency first using the link: `https://github.com/GiovanniZambiasi/middlemast-core.git`
1. Then, install this package: `https://github.com/GiovanniZambiasi/gameplay-framework-unity.git`

# Callbacks

This framework defines *custom callbacks*. This is done to avoid the inconsistencies and performance concerns of Unity's built-in *life-cycle callbacks\**. Unity callbacks should be avoided as often as possible.

<a name="life-cycle-callbacks">\* *Unity's life-cycle callbacks are:* `Awake`, `Start`, `Update`/`LateUpdate`/`FixedUpdate`, and `OnDestroy`</a>

## Custom callback cheat sheet

| Keyword | Corresponding Unity Callback |
| :---: | :---: |
| Setup | Start/Awake |
| Dispose | OnDestroy |
| Tick | Update |

## Why custom callbacks?

Having manual control over the order of the *setup/update/dispose* of your classes can be **very beneficial**. This avoids race-conditions between `MonoBehaviours` (ever had a bug where some `MonoBehaviour`'s `Start` method got called before another's, and the former depended on the latter to initialize itself?). It also enables any developer in the project to understand **exactly in what order** things are happenning, without having to worry about the [Script Execution Order settings](https://docs.unity3d.com/Manual/class-MonoManager.html) hidden away in a menu. Defining custom `Tick(float deltaTime)` methods can also be extremely useful when writing [unit tests](https://docs.unity3d.com/2017.4/Documentation/Manual/testing-editortestsrunner.html).

# Rules

### Severity guide
Severity | Description
:---: | :---:
🟥 | **Severe**. Must always be followed
🟨 | **Encouraged**. Should mostly be followed, but can have exceptions
🟩 | **Suggestion**. May not apply to a specific project

### General rules
Rule | [Severity](#severity-guide)
:--- | :---:
Keep the root namespace of an assembly the same as the assembly name | 🟥
The `namespaces` in your scripts must match your folder structure, taking the root namespace of the assembly into consideration | 🟥
All of your project's scripts must live under the same root folder | 🟨
All assemblies must live inside their own folder. And that folder must live in the root of your scripts folder. **You cannot have assemblies inside other assemblies**\* | 🟨

*\* Here's an example of an ideal assembly setup:*
```
_Project/
    '-Scripts/
        '-WizardsAndGoblins/
            '-WizardsAndGoblins.asmdef
        '-WizardsAndGoblins.Tests/
            '-WizardsAndGoblins.Tests.asmdef
        '-WizardsAndGoblins.Editor/
            '-WizardsAndGoblins.Editor.asmdef
        '-WizardsAndGoblins.Editor.Tests/
            '-WizardsAndGoblins.Editor.Tests.asmdef
        '-WizardsAndGoblins.Gameplay/
            '-WizardsAndGoblins.Gameplay.asmdef
        '-WizardsAndGoblins.Gameplay.Test/
            '-WizardsAndGoblins.Gameplay.Test.asmdef
        '-WizardsAndGoblins.Gameplay.Editor/
            '-WizardsAndGoblins.Gameplay.Editor.asmdef
    // etc..
```

# Systems
A `System` is an **entry point with the engine**. It's meant to be an autonomous class that initializes/disposes/updates itself through [*Callbacks*](#callbacks). `Systems` can be big or small, and you can define as many as you like. While designing `Systems`, try to find *isolated chunks of behaviour* in your project. This will help keep related things together, and will naturally avoid making a mess with your scripts and `namespaces`. `Systems` must be **as indepentent from one another as possible**. For a *Counter-Strike* style game, you could define the gameplay portion as a `GameplaySystem`, and the Menus and matchmaking as a `MenuSystem`, for example.

## Rules

Rule | [Severity](#severity-guide)
:--- | :---:
Each `System` must be inside it's own `namespace` | 🟥
Each `System` must be in it's own `GameObject` | 🟥
**Communication between `Systems` must always be abstracted** (a `System` shouldn't have *any* knowledge about another `System`) | 🟥
Each `System` should be in a separate [Assembly](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) | 🟨
Keep all `Systems` at the root of a *Scene* | 🟨
Make all your `Systems` [`internal`](https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/internal)\* | 🟨
Add the `System` suffix to all `Systems` (and the `GameObjects`' names should match the type names) | 🟩    

*\* For unit testing purposes, you can use the `InternalsVisibleTo` attribute to give your test assemblies access to your `System`*

## Encapsulation

A `System` can encapsulate it's behaviour using `Managers`. It can have any number of them, and their lifetimes will be managed by their owning `System`. They have generic callbacks for `Setup`, `Dispose` and `Tick(float deltaTime)`, but can implement any overloads your project requires. `Managers` can be added to a system using the *hierarchy* of a *Unity scene*. Any `GameObject` with a `Manager` component **that's a child of a `System`** will be registered:

<a name="managers-hierarchy">![image](https://user-images.githubusercontent.com/46461122/152656464-d37024dc-b370-4d74-8fb4-e41ed753a112.png)</a>

During `Setup`, the `System` will initialize each `Manager`, in the exact order of the hierarchy. In the example above, `EarlyManager` will be setup *first*, and `LateManager` will be setup *last*. This is also the order in which the `Tick` funcitions will get called.
`Dispose` is a bit different. When the `System` is disposing it's `Managers`, this will happen in the **reverse order**. In the example above, `LateManager` will be the *first disposed*, and `EarlyManager` will be *last*. In most cases, this is the desired behaviour.

# Managers
A `Manager` is the basic *building-block* of a `System`. The main difference between `Systems` and `Managers` is that **`Managers` are not autonomous**. This means that their life-cycles must be managed entirely by a `System`. In other words, they must not implement any [*life-cycle related*](#life-cycle-callbacks) Unity callbacks. `Managers` already implement default life-cycle callbacks, but more callbacks can be defined to match your game's needs. In a turn-based game, you could define `TurnEnd` and `TurnStart` callbacks, passed along to your `Managers` by some `GameplaySystem`, for example.

## Rules

Rule | [Severity](#severity-guide)
:--- | :---:
**Communication between `Managers` must always be abstracted (a `Manager` shouldn't have any knowledge about another `Manager`)** | 🟥
`Managers` must define their own `namespaces`\*| 🟥
`Managers` **must not implement any [*life-cycle related*](#life-cycle-callbacks)** Unity callbacks | 🟥
Each `Managers` must live in it's own `GameObject`, and must be a [first-level-child](#managers-hierarchy) of a `System` | 🟥
Make all your `Managers` [`internal`](https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/internal)\*\* | 🟨
Add the `Manager` suffix to all `Managers` (and their `GameObject`'s names should match their type names) | 🟩  

*\* Example:*  
![Anatomy of a System](https://user-images.githubusercontent.com/46461122/153716988-a2d6eb92-67e9-4240-bdd6-eb00f461ae6f.png)
*In the diagram above, `Market` is the root namespace of the `System`. Each `Manager` defines it's own child-namespace, and must never reference one another*

*\*\* For unit testing purposes, you can use the `InternalsVisibleTo` attribute to give your test assemblies access to your `Manager`*

## Managing dependencies
`Managers` are likely to have varying dependencies that must be fulfilled during their `Setup`. You could have a `StoreManager` which needs a reference to a `StoreData` `ScriptableObject`, for example. In this case, the default `Setup` callback may not be so useful, and *custom overloads* should be defined. It's encouraged to use the same naming for your default callback overloads. For example, the `StoreManager` could have a `Setup(StoreData storeData)` overload for the default `Setup` callback. The `System` could then *override* the `SetupManagers` method, and call the overloaded version of `Setup`:
```cs
public class MenuSystem : System
{
    [SerializeField] private StoreData _storeData;

    protected override void SetupManagers()
    {
        base.SetupManagers();

        StoreManager storeManager = GetManager<StoreManager>();
        storeManager.Setup(_storeData);
    }
}
```
# Entities
`Entities` are the individual objects that make up your gameplay. A `Zombie`, the `Player` or a `WoodenBox` could all be considered entities. Most games organize their entities into `Prefabs`, and these objects can be spawned at runtime, or be dragged into a scene beforehand. They can have various `Components`, such as a `Rigidbody`, `HealthComponent` or an `Animator`. However, every `Entity` must be defined as it's own `MonoBehaviour`:
```cs
public class Wizard : Entity
{
    // Magic...
}
```

## Rules
Rule | [Severity](#severity-guide)
:--- | :---:
`Entities` cannot live in the root `namespace` of an assembly | 🟥
`Entities` **must not implement any [*life-cycle related*](#life-cycle-callbacks)** Unity callbacks | 🟥
Each `Entity` must live in it's own `GameObject` | 🟥
**Communication between different entities must always be abstracted** (an `Entity` shouldn't have any knowledge about another `Entity`) | 🟥
Each `Entity` must have a corresponding `Manager`\* | 🟨
Make all your `Entities` [`internal`](https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/internal)\*\* | 🟨

*\* There could be some cases where a `System` is simple enough that it can manage entities by itself*  
*\*\*For unit testing purposes, you can use the `InternalsVisibleTo` attribute to give your test assemblies access to your `Entity`**

## What about composition?
Composition should always be strived for. However, in some cases, we need a "central point" for our game's entities to orchestrate *complex interactions* between `Components`. For example: Say you had a `HealthComponent` and a `MovementComponent`. You want to reduce your `Player`'s movement speed when their health drops below 50%. To achieve this, you need some sort of communication between those two components. That's where the `Player` `Entity` comes in:
```cs
public class Player : Entity
{
    private HealthComponent _health;
    private MovementComponent _movement;

    public override void Setup()
    {
        base.Setup();

        _health.OnDamageTaken += HandleDamageTaken;
    }

    private void HandleDamageTaken()
    {
        if (_health.HealthRatio <= .5f)
        {
            _movement.Speed *= .66f;
        }
    }
}
```
It's a central point of communication between the two. That way, your `HealthComponent` only needs to worry about damage and health, and your `MovementComponent` only needs to worry about translation and physics. 

With good abstraction, you *could* achieve this "slow-down-when-almost-dying" behaviour **without the need for a `Player`**. If your code is modular enough that it doesn't need the `Entity` to be defined in code, then don't define it. `Entities` will be most useful for objects that have a high number of `Components`, all performing complex interactions with one another.

## So how does a Wizard cast a Fireball?
Later in this document, there's an [example](#wizards-and-goblins) of how to implement a `Wizard` that can cast `Fireballs` at `Goblins` following all the rules of this framework.

# Components
Components are tiny, autonomous, encapsulated slices of behaviour that live inside your `Systems`, `Managers` or `Entities`. In fact, any `MonoBehaviour` *is considered* a component by the engine. This doesn't mean that all `MonoBehaviours` you create are components *from a conceptual standpoint*. `Systems`, `Managers` or `Entities`, for example, are *not* components. Most components are modular by nature. Some are so modular that they can just be added into an object and they'll work, without any need for external help. This is a **great resource**, and it should be leveraged extensively. Since we want to preserve the modular and autonomous nature of *most* components, they **can** implement [Unity's life-cycle callbacks](#life-cycle-callbacks).

## Rules
Rule | [Severity](#severity-guide)
:--- | :---:
Add the `Component` suffix to all your components | 🟩

## Creating a Component
To create a component, simply make a `MonoBehaviour` how you usually would. A great example of a modular component is the `Rigidbody`. You can just add it to any object and they'll fall downwards (or upwards), depending on your gravity settings. If you want your components to be managed by their owning object, you can do so. It's up to the developer to decide whether the component is in fact autonomous or not:

Take a `BillboardComponent`, for example. It can simply point a transform towards the main camera on `Update`. This is a component that *can* be autonomous:
```cs
namespace DogsAndBillboards
{
    public class BillboardComponent : MonoBehaviour
    {
        private void LateUpdate()
        {
            Camera mainCamera = Camera.main;
            Vector3 lookDirection = (mainCamera.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
```  
This component *doesn't need* to be managed. It can resolve its dependencies by itself, and doesn't communicate with any other `MonoBehaviours`. This is not always the case:
Say we have a `Dog` `Entity` with a `DogAnimations` component. The `Dog` has a `BreedData` `ScriptableObject` that contains important information. The `DogAnimation` component needs to be setup by it's owning `Dog`, so it can initialize itself with the correct `BreedData`:
```cs
namespace DogsAndBillboards.Dogs
{
    public class BreedData : ScriptableObject
    {
        // Information about size, sound and animations...
    }
    
    public class DogAnimations : MonoBehaviour
    {
        public void Setup(BreedData data)
        {
            // Initializes itself based on the breed
        }
    }
    
    public class Dog : Entity
    {
        private DogAnimations _animations;

        public void Setup(BreedData breed)
        {
            _animations = GetComponent<DogAnimations>();
            _animations.Setup(breed);
        }
    }
}
```  
In this case `DogAnimations` is still a component, but it's managed by an `Entity`.

# Abstraction
Abstraction *is very important*. It's an integral part of writing clean code, and it's all about defining *how much `types` should know about each other*. The less they know, the more decoupled your codebase gets. Decoupling is very useful, as it enables developers to write clean, encapsulated code that is easily **changed, tested and understood.** Thus, in most cases, abstraction *should be encouraged*. However, as important as it may be, abstraction can be quite complicated to apply consistently.

## When do I use abstraction?
The rules from the previous chapters were crafted to answer this question. This framework defines `namespaces` as *layers of abstraction*. The idea is that each `namespace` defines a *system boundary*. Whenever you need to have communication between two separate `namespaces`, abstraction should be applied. This boils down to a simple rule, that can be easily followed:

- **A `Type` can only know about another `Type` if they're in the same `namespace`** 🟥
    - This applies hierarchically, so nested `namespaces` *can know about* `Types` in their parent `namespace`
    - This doesn't mean that you shouldn't use abstraction within a particular `namespace`. It can, and should, be considered

From here onwards, the above rule will be referred to as **the namespaces rule**. Combining it with the rules from the other chapters will help you apply abstraction to your codebase **very consistently**. Here's an example:

![Abstraction map](https://user-images.githubusercontent.com/46461122/152663172-970889fc-f20d-4f28-bf84-33a00ca6ffa9.png)

In the image above, the dotted lines represent a map of *who knows whom*. Notice how all the lines coming from the nested namespace (`TheAncientScrolls.Dragons`) have arrows, symbolizing that they only go one way. In the parent namespace (`TheAncientScrolls`), there are 3 types: `AttributeSet`, `IInteractable`, `ICatchFire`. They can all know about each other, as they're in the same `namespace`. However, they **can't know about** any types inside `TheAncientScrolls.Dragons`. On the other hand, `TheAncientScrolls.Dragons` is still a part of `TheAncientScrolls`. Therefore, types inside `TheAncientScrolls.Dragons` **can know about** types in `TheAncientScrolls`.

## Exceptions
The `namespaces` rule will be most useful for your *runtime* scripts, where most of the buisness logic of your application will go. However, exceptions can be made for **`Editor` and `Test`** assemblies, since those namespaces are technically different than their respective types. For example, if you have a `Wizard : Entity` in a `WizardsAndGoblins.Wizards` `namespace`, an editor for it would likely be called `WizardEditor` inside a `WizardsAndGoblins.Editor.Wizards` `namespace`. In this case `WizardEditor` *needs to know* about the type in `WizardsAndGoblins.Wizards`.

Another exception to the rule are `Systems`. Since they will be responsible for all `Managers` and their `Entities`, forbidding access their types could become counter-productive. If you deem it neccessary, you *can* apply abstraction between the `System` and its `Managers`. This could be useful if you want to have different `Managers` on different levels of you game, but still maintain the same `System`. For example, you could have an interface `IObjectiveManager` that has two implementations: `CaptureTheFlagManager` and `TeamDeathmatchManager`, one for each game mode. The `System` could use that interface to communicate with them, preserving abstraction between the two.

## So who can go in the root `namespace`?
The root `namespace` of an assembly should be reserved mostly to interfaces, shared data objects, extension methods and some components. **No `Entity` or `Manager`** should be in the root `namespace` of an assembly.

## How do I abstract?
There are many ways of achieving abstraction, but here are some examples involving `Managers` and a `System`:
- Using [C# events](https://docs.microsoft.com/en-us/dotnet/standard/events/):
  - A `Manager` could declare a specific `event` for when something happens. The `System` could subscribe to this event, and pass the callback along to another `Manager`  
![Abstraction with CSharp Events](https://user-images.githubusercontent.com/46461122/152659476-24bb47ae-e87f-48e6-ba9b-e5bf1312619b.png)

- Using [interfaces](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface):
  - A `Manager` can implement some `IDoesSomething` *interface*, and another `Manager` could define a `Setup(IDoesSomething dependency)`, which gets fulfilled by the `System`  
![Abstraction with Interface](https://user-images.githubusercontent.com/46461122/152659540-2b6e00ea-af6c-46d0-9168-ba0227b7b084.png)  
*For a more in-depth look, open the Abstraction with Interface example in this repository*

- Using some sort of event bus, to achieve complete abstraction between an event's *sender* and *listener*. [Here's](https://www.youtube.com/watch?v=WLDgtRNK2VE) a good example of how to implement one

# Examples

## Wizards and Goblins
Our goal in this chapter is to make a `Wizard` that can cast a `Fireball` at a `Goblin`. This should provide a good understanding of how the framework is meant to be used.

This example requires 3 `Entities` with corresponding `Manager`s (and therefore, 3 nested namespaces):  
![image](https://user-images.githubusercontent.com/46461122/152685659-967cbe6e-41d0-4ed8-9ec9-218fc611a48b.png)  
*Consider each folder in the example a C# `namespace`*

### Casting a fireball
Now, the `Wizard` needs to be able to cast a `Fireball`, but they're in separate namespaces. Simply including the `using WizardsAndGoblins.Spells` directive in any of the `Wizard`'s scripts would be a violation of the [namespaces rule](#when-do-i-use-abstraction). This is where abstraction comes into play:

We need to define a communication layer between `Wizards` and their `Spells`. For that, we will declare an interface:
```cs
namespace WizardsAndGoblins     // Notice how the interface is declared in the root namespace of the assembly
{
    public interface ISpell
    {
        void Activate();
    }
}
```
Then, `Fireball` must implement this interface:
```cs
namespace WizardsAndGoblins.Spells
{
    internal class Fireball : Entity, ISpell
    {
        private Rigidbody _rigidbody;

        public override void Setup()
        {
            base.Setup();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Activate()
        {
            _rigidbody.AddRelativeForce(transform.forward * 10f, ForceMode.Impulse);
        }
    }
}
```
For now, the `Activate` method is all the `Wizard` needs to be able to communicate with it's `Fireball`. Now, *how do we create an `Entity` from another `Entity`*? For that, we will need another interface:
```cs
namespace WizardsAndGoblins
{
    public interface ISpellFactory
    {
        ISpell CreateSpell(Vector3 position, Vector3 direction);
    }
}
```
And we can make the `SpellManager` implement it:
```cs
namespace WizardsAndGoblins.Spells
{
    internal class SpellManager : Manager, ISpellFactory
    {
        [SerializeField] private GameObject _spellPrefab;
    
        private List<Entity> _spells = new List<Entity>();

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            for (int i = 0; i < _spells.Count; i++)
            {
                Entity spell = _spells[i];
                spell.Tick(deltaTime);
            }
        }

        public ISpell CreateSpell(Vector3 position, Vector3 direction)
        {
            if (!_spellPrefab.TryGetComponent(out ISpell spell))
            {
                Debug.LogError($"Prefab '{_spellPrefab.name}' is not a spell!");
                return null;
            }

            spell = Instantiate(_spellPrefab, position, Quaternion.LookRotation(direction)).GetComponent<ISpell>();

            if (spell is Entity entity)
            {
                _spells.Add(entity);
            }

            return spell;
        }
    }
}
```
I have also added some code that makes the `SpellManager` register spells, and update them using `Tick`. 

With `ISpellFactory`, we managed to create an abstract way to spawn instances of `Fireball`. Now, we can define the `Wizard`'s `CastSpell` method:
```cs
namespace WizardsAndGoblins.Wizards
{
    internal class Wizard : Entity
    {
        private ISpellFactory _spellFactory;

        public void Setup(ISpellFactory spellFactory)
        {
            _spellFactory = spellFactory;
        }

        public void CastSpell(Vector3 direction)
        {
            ISpell spell = _spellFactory.CreateSpell(transform.position, direction);
            spell.Activate();
        }
    }
}
```
Notice how the `Wizard`'s dependency to `ISpellFactory` is fulfilled in the `Setup` method overload. This is how the `WizardManager` does that:
```cs
namespace WizardsAndGoblins.Wizards
{
    internal class WizardManager : Manager
    {
        [SerializeField] private Wizard _wizardPrefab;

        private ISpellFactory _spellFactory;
        private Wizard _wizard;

        public void Setup(ISpellFactory spellFactory)
        {
            _spellFactory = spellFactory;

            CreateWizard();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            _wizard.Tick(deltaTime);
        }

        private void CreateWizard()
        {
            _wizard = Instantiate(_wizardPrefab);
            _wizard.Setup(_spellFactory);
        }
    }
}
```
And the `WizardManager`'s dependencies are fulfilled by the `GameplaySystem`:
```cs
namespace WizardsAndGoblins
{
    internal class GameplaySystem : System
    {
        protected override void SetupManagers()
        {
            base.SetupManagers();

            SpellManager spellManager = GetManager<SpellManager>();

            WizardManager wizardManager = GetManager<WizardManager>();
            wizardManager.Setup(spellManager);
        }
    }
}
```
And *voila! Just like magic,* the `Wizard` is able to cast a `Fireball`, without any code coupling. Now, why go through all the trouble?

A great benefit of all this infrastructure is this: Notice how the `Wizard` `Entity` has no idea of **what the spell is**, or **what it does**. If we wanted to completely change what spell the `Wizard` casts, we could easily do so, without even having to open the `Wizard`'s script for editing. All that we would need to do is define another `ISpell` `Entity` with a different behaviour. A `Heal` spell, for example, could look like this:
```cs
namespace WizardsAndGoblins.Spells
{
    internal class Heal : Entity, ISpell
    {
        public void Activate()
        {
            // Finds nearby healable objects using collision, and add some value to their health
        }
    }
}
```
Also, notice how neither the `Wizard` nor the `WizardManager` know how a spell is *created, managed or destroyed*. This gives us the flexibility of using whichever infrastructure we need for `ISpell`. For example, if we had to use a pre built spell plugin from the asset store, we could define `ISpell` and `ISpellFactory` implementations to communicate with it, and the rest of our codebase wouldn't even know. That is *✨the power of abstraction ✨*

### Damaging the Goblin
// Tbd..
