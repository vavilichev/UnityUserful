# Storage system for Unity

• Async operation supported (with callbacks)<br>
• Coroutine operation supported (with callbacks also)<br>
• Only local (file) storage realized now<br>
• The system is flexible and support expanding with a cloud storage really easy<br>
• Mobile friendly<br>
• Multiple storage containers supported<br>
<br>

### v.3.0
Even more flexible aproach to save the  game data.<br>
**WARNING: If you want to update your storage system from v.2.12 to v.3.0, you will have to make some architecture changes. You cannot just update the asset.**
<br>

Whats new:
- You can save different blocks of data. It works like a container. Just create Storage instance (FileStorage or Cloud Storage (not implemented yet)) and save data into it. It will be convenient to separate game settings data that usually loads at the start of the game from game progress data that usually loads after main menu scene.



### v2.12
More flexible approach to save the game data with key-value pair. Looks like Player Prefs, but it isn't the same. New version supports all serializable objects and stored in one place (file or cloud, cloud doesn't supported yet).
