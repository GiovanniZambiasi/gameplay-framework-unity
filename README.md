# Gio's Gameplay Framework for Unity

A solid, battle-tested framework to architect games in Unity. This repository is meant to be used as a [UPM package](https://docs.unity3d.com/Manual/upm-ui.html).

# Summary

## Installation:
Install via UPM using the link: `https://github.com/GiovanniZambiasi/gameplay-framework-unity.git`

## Callbacks

This framework defines *custom callbacks*. This is done to avoid the inconsistencies and performance concerns of Unity's built-in *life-cycle callbacks\**. Unity callbacks should be avoided as often as possible.

<a name="life-cycle-callbacks">\* *Unity's life-cycle callbacks are:* `Awake`, `Start`, `Update`/`LateUpdate`/`FixedUpdate`, and `OnDestroy`</a>

### Custom callback cheat sheet

| Keyword | Corresponding Unity Callback |
| :---: | :---: |
| Setup | Start/Awake |
| Dispose | OnDestroy |
| Tick | Update |

### Why custom callbacks?

Having manual control over the order of the *setup/update/dispose* of your classes can be **very beneficial**. This avoids race-conditions between `MonoBehaviour`s (ever had a bug where some `MonoBehaviour`'s `Start` method got called before another's, and the former depended on the latter to initialize itself?). It also enables any developer in the project to understand **exactly in what order** things are happenning, without having to worry about the [Script Execution Order settings](https://docs.unity3d.com/Manual/class-MonoManager.html) hidden away in a menu. Defining custom `Tick(float deltaTime)` methods can also be extremely useful when writing [unit tests](https://docs.unity3d.com/2017.4/Documentation/Manual/testing-editortestsrunner.html).

## Rules

### Severity guide
Severity | Description
:---: | :---:
🟥 | **Severe**. Must always be followed
🟨 | **Encouraged**. Should mostly be followed, but can have exceptions
🟩 | **Suggestion**. May not apply to a specific project

### General rules
Rule | [Severity](#severity-guide)
:--- | :---:
All of your project's scripts must live under the same root folder | 🟨
All assemblies must live inside their own folder. And that folder must live in the root of your scripts folder. **You cannot have assemblies inside other assemblies**\* | 🟨
Keep the root namespace of an assembly the same as the assembly name | 🟨
The `namespaces` in your scripts must match your folder structure, taking the root namespace of the assembly into consideration | 🟨

*\* Example:*
```
_Project/
    '-Scripts/
        '-WizardsAndGoblins/
            '-WizardsAndGoblins.asmdef
        '-WizardsAndGoblins.Editor/
            '-WizardsAndGoblins.Editor.asmdef
        '-WizardsAndGoblins.Gameplay/
            '-WizardsAndGoblins.Gameplay.asmdef
    // etc..
```

# Systems
A `System` is an **entry point with the engine**. It's meant to be an autonomous class that initializes/disposes/updates itself through [*Callbacks*](#callbacks). `System`s can be big or small, and you can define as many as you like. While designing `System`s, try to find *isolated chunks of behaviour* in your project. This will help keep related things together, and will naturally avoid making a mess with your scripts and `namespace`s. `System`s must be **as indepentent from one another as possible**. For a *Counter-Strike* style game, you could define the gameplay portion as a `GameplaySystem`, and the Menus and matchmaking as a `MenuSystem`, for example.

## Rules

Rule | [Severity](#severity-guide)
:--- | :---:
Each `System` must be inside it's own `namespace` | 🟥
Each `System` must be in it's own `GameObject` | 🟥
**Communication between `Systems` must always be abstracted** (a `System` shouldn't have *any* knowledge about another `System`) | 🟥
Each `System` should be in a separate [Assembly](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) | 🟨
Keep all `System`s at the root of a *Scene* | 🟨
Make all your `System`s [`internal`](https://docs.microsoft.com/pt-br/dotnet/csharp/language-reference/keywords/internal)\* | 🟨
Add the `System` suffix to all `Systems` (and the `GameObjects`' names should match the type names) | 🟩

  
*\* For unit testing purposes, you can use the `InternalsVisibleTo` attribute to give your test assemblies access to your `System`*

## Encapsulation

A `System` can encapsulate it's behaviour using `Manager`s. It can have any number of them, and their lifetimes will be managed by their owning `System`. They have generic callbacks for `Setup`, `Dispose` and `Tick(float deltaTime)`, but can implement any overloads your project requires. `Manager`s can be added to a system using the *hierarchy* of a *Unity scene*. Any `GameObject` with a `Manager` component **that's a child of a `System`** will be registered:

<a name="managers-hierarchy">![image](https://user-images.githubusercontent.com/46461122/152656464-d37024dc-b370-4d74-8fb4-e41ed753a112.png)</a>

During `Setup`, the `System` will initialize each `Manager`, in the exact order of the hierarchy. In the example above, `EarlyManager` will be setup *first*, and `LateManager` will be setup *last*. This is also the order in which the `Tick` funcitions will get called.
`Dispose` is a bit different. When the `System` is disposing it's `Managers`, this will happen in the **reverse order**. In the example above, `LateManager` will be the *first disposed*, and `EarlyManager` will be *last*. In most cases, this is the desired behaviour.

# Managers
A `Manager` is the basic *building-block* of a `System`. The main difference between `System`s and `Manager`s is that **`Manager`s are not autonomous**. This means that their life-cycles must be managed entirely by a `System`. In other words, they must not implement any [*life-cycle related*](#life-cycle-callbacks) Unity callbacks. `Manager`s already implement default life-cycle callbacks, but more callbacks can be defined to match your game's needs. In a turn-based game, you could define `TurnEnd` and `TurnStart` callbacks, passed along to your `Manager`s by some `GameplaySystem`, for example.

## Rules

Rule | [Severity](#severity-guide)
:--- | :---:
**Communication between `Manager`s must always be abstracted (a `Manager` shouldn't have any knowledge about another `Manager`)** | 🟥
`Manager`s must define their own [`namespaces`]\*| 🟥
`Manager`s **must not implement any [*life-cycle related*](#life-cycle-callbacks)** Unity callbacks | 🟥
Each `Manager`s must live in it's own `GameObject`, and must be a [first-level-child](#managers-hierarchy) of a `System` | 🟥
Add the `Manager` suffix to all `Managers` (and their `GameObject`'s names should match their type names) | 🟩

*\* Example:*  
![Anatomy of a System](https://user-images.githubusercontent.com/46461122/152659092-e5dedab5-48a6-431c-8f3b-8281e3120fa9.png)  
*In the diagram above, `Market` is the root namespace of the `System`. Each `Manager` defines it's own child-namespace, and must never reference one another*

## Managing dependencies
`Manager`s are likely to have varying dependencies that must be fulfilled during their `Setup`. You could have a `StoreManager` which needs a reference to a `StoreData` `ScriptableObject`, for example. In this case, the default `Setup` callback may not be so useful, and *custom overloads* should be defined. It's encouraged to use the same naming for your default callback overloads. For example, the `StoreManager` could have a `Setup(StoreData storeData)` overload for the default `Setup` callback. The `System` could then *override* the `SetupManagers` method, and call the overloaded version of `Setup`:
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
Entities are the individual objects that make up your gameplay. A `Zombie`, the `Player` or a `WoodenBox` could all be considered entities. Most games organize their entities into `Prefabs`, and these objects can be spawned at runtime, or be dragged into a scene beforehand. They can have various `Component`s, such as a `Rigidbody`, `HealthComponent` or an `Animator`. However, every `Entity` must be defined as it's own `MonoBehaviour`:
```cs
public class Wizard : Entity
{
    // Magic...
}
```

## Rules
Rule | [Severity](#severity-guide)
:--- | :---:
Entities cannot live in the root `namespace` of an assembly | 🟥
Entities **must not implement any [*life-cycle related*](#life-cycle-callbacks)** Unity callbacks | 🟥
Each `Entity` must live in it's own `GameObject` | 🟥
**Communication between different entities must always be abstracted** (an `Entity` shouldn't have any knowledge about another `Entity`) | 🟥
Each `Entity` must have a corresponding `Manager`\* | 🟨

*\* There could be some cases where a `System` is simple enough that it can manage entities by itself*

## What about composition?
Composition should always be strived for. However, in some cases, we need a "central point" for our game's entities to orchestrate *complex interactions* between `Component`s. For example: Say you had a `HealthComponent` and a `MovementComponent`. You want to reduce your `Player`'s movement speed when their health drops below 50%. To achieve this, you need some sort of communication between those two components. That's where the `Player` `Entity` comes in:
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
It's a central point of communication between the two. That way, your `HealthComponent` only needs to wory about damage and health, and your `MovementComponent` only needs to worry about translation an physics. 

With good abstraction, you *could* achieve this "slow-down-when-almost-dying" behaviour **without the need for a `Player`**. If your code is modular enough that it doesn't need the `Entity` to be defined in code, there's no need to define one. `Entities` will be most useful for objects that have a high number of `Component`s, all performing complex interactions with one another.

## So how does a Wizard cast a Fireball?
Later in this document, there's an [example](#wizards-and-goblins) of how to implement a `Wizard` that can cast `Fireballs` at `Goblins` following all the rules of this framework.

# Abstraction
As mentioned before: *abstraction is important* and, in most cases, *should be encouraged*. It enables developers to write clean, encapsulated code that is easily **understood, managed and tested.** However, as important as it may be, abstraction can also be quite complicated to apply consistently.

## When do I use abstraction?
Provided you've followed all the rules from the chapters above, a good guideline you can follow when it comes to abstraction is this:

- **Types should only know about other types in their own `namespace`s**

This doesn't mean that you shouldn't use abstraction within a particular `namespace`. It can, and should, be considered. However, the above quote should be followed *as a rule*. Here's an example:

![Abstraction map](https://user-images.githubusercontent.com/46461122/152663172-970889fc-f20d-4f28-bf84-33a00ca6ffa9.png)

In the image above, the dotted lines represent a map of *who knows whom*. Notice how all the lines coming from the inner namespace `TheAncientScrolls.Dragons` have arrows, symbolizing that they only go one way. In the root ``namespace``, there are 3 types: `AttributeSet`, `IInteractable`, `ICatchFire`. They can all know about each other, as they're in the same `namespace`. However, they **can't know about** any types inside `TheAncientScrolls.Dragons`. On the other hand, `TheAncientScrolls.Dragons` is still a part of `TheAncientScrolls` `namespace`. Therefore, all types inside `TheAncientScrolls.Dragons` **can know about** any types in `TheAncientScrolls`.

This guideline, in combination with the namespace and assembliy rules from the previous chapters, should make for a very organized and well abstracted codebase.

## So who can go in the root `namespace`?
The root `namespace` of an assembly should be reserved mosttly to interfaces, shared data objects, extension methods and some components. **No `Entity` or `Manager`** should be in the root `namespace` of an assembly.

## How do I abstract?
There are many ways of achieving abstraction, but here are some examples involving `Managee`s and a `System`:
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

### First things first
This example requires 3 `Entities` with corresponding `Manager`s (and therefore, 3 nested namespaces):  
![image](https://user-images.githubusercontent.com/46461122/152685659-967cbe6e-41d0-4ed8-9ec9-218fc611a48b.png)  
*Consider each folder in the example a C# `namespace`*

Now, the `Wizard` needs to be able to cast a `Fireball`, but they're in separate namespaces


