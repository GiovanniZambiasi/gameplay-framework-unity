# Gio's Gameplay Framework for Unity

A solid, battle-tested framework to architect games in Unity.

# Summary

## Callbacks <a name="Callbacks"></a>

This framework defines *custom callbacks*. This is done to avoid the inconsistencies and performance concerns of Unity's built-in callbacks. Unity callbacks should be avoided as often as possible.

### Custom callback cheat sheet

| Keyword | Corresponding Unity Callback |
| :---: | :---: |
| Setup | Start/Awake |
| Dispose | OnDestroy |
| Tick | Update |

# Systems
A `System` is an **entry point with the engine**. It's meant to be an autonomous class that initializes/disposes/updates itself through [*Callbacks*](Callbacks). `Systems` can be big or small, and you can use as many as you like. While defining `System`s, try to create *isolated chunks of behaviour*. Try to keep `System`s **as indepentent to one another as possible**. For a *Counter-Strike* style game, you could define the gameplay portion as a `GameplaySystem`, and the Menus and matchmaking as a `MenuSystem`, for example. **Communication between `Systems` must always be abstracted** (a `System` shouldn't have *any* knowledge about another `System`).

A `System` can encapsulate it's behaviour inside `Manager`s. It can have any number of them. Their lifetimes will be managed by their owning `System`. They have generic callbacks for `Setup`, `Dispose` and `Tick(float deltaTime)`, but can implement any overloads the project requires. `Manager`s can be added to a system using the *hierarchy* of a *Unity scene*. Any `GameObject` with a `Manager` component **that's a child of a `System`** will be registered:
![image](https://user-images.githubusercontent.com/46461122/152656464-d37024dc-b370-4d74-8fb4-e41ed753a112.png)
Their order of initialization and update will be called 
All 3 `Manager`s in the scene have a `Manager` component assigned to it
**immediate** child objects.
