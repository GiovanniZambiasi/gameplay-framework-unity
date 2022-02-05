# Gio's Gameplay Framework for Unity

A solid, battle-tested framework to architect games in Unity.

# Summary

## Callbacks

This framework defines *custom callbacks*. This is done to avoid the inconsistencies and performance concerns of Unity's built-in callbacks. Unity callbacks should be avoided as often as possible.

### Custom callback cheat sheet

| Keyword | Corresponding Unity Callback |
| :---: | :---: |
| Setup | Start/Awake |
| Dispose | OnDestroy |
| Tick | Update |

### Why custom callbacks?

Having manual control over the order of the *setup/update/dispose* of your classes can be **very beneficial**. This avoids race-conditions between `MonoBehaviour`s (ever had a bug where some `MonoBehaviour`'s `Start` method got called before another's, and the former depended on the latter to initialize itself?). It also enables any developer in the project to understand **exactly in what order** things are happenning, without having to worry about the [Script Execution Order settings](https://docs.unity3d.com/Manual/class-MonoManager.html) hidden away in a menu. Defining custom `Tick(float deltaTime)` methods can also be extremely useful when writing [unit tests](https://docs.unity3d.com/2017.4/Documentation/Manual/testing-editortestsrunner.html).

# Systems
A `System` is an **entry point with the engine**. It's meant to be an autonomous class that initializes/disposes/updates itself through [*Callbacks*](#callbacks). `System`s can be big or small, and you can define as many as you like. While designing `System`s, try to find *isolated chunks of behaviour* in your project. This will help keep related things together, and will naturally avoid making a mess with your scripts and `namespace`s. It's encouraged to put each `System` in a separate [Assembly](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) (and therefore a separate `namespace`). Try to keep `System`s **as indepentent from one another as possible**. For a *Counter-Strike* style game, you could define the gameplay portion as a `GameplaySystem`, and the Menus and matchmaking as a `MenuSystem`, for example. **Communication between `Systems` must always be abstracted** (a `System` shouldn't have *any* knowledge about another `System`).

A `System` can encapsulate it's behaviour using `Manager`s. It can have any number of them, and their lifetimes will be managed by their owning `System`. They have generic callbacks for `Setup`, `Dispose` and `Tick(float deltaTime)`, but can implement any overloads your project requires. `Manager`s can be added to a system using the *hierarchy* of a *Unity scene*. Any `GameObject` with a `Manager` component **that's a child of a `System`** will be registered:
![image](https://user-images.githubusercontent.com/46461122/152656464-d37024dc-b370-4d74-8fb4-e41ed753a112.png)
During `Setup`, the `System` will initialize each `Manager`, in the exact order of the hierarchy. In the example above, `EarlyManager` will be setup *first*, and `LateManager` will be setup *last*. This is also the order in which the `Tick` funcitions will get called.
`Dispose` is a bit different. When the `System` is disposing it's `Managers`, this will happen in the **reverse order**. In the example above, `LateManager` will be the *first disposed*, and `EarlyManager` will be *last*. In most cases, this is the desired behaviour.
