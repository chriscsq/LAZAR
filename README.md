# LAZAR
AR Tower defense game made with Unity and Vuforia

Unity version: 3.7.f1

# How To Build
Open up the project in Unity. Assets/Scenes/HomeScreen.unity and Assets/Scenes/GameScene.unity are the only "true" scenes; the others are outdated that we've kept around for reference, though likely prefabs have changed enough that they don't work anymore.

So, to build, make sure the above two scenes are included in the build and try to create a build for your platform. We've tested both Android and iOS, and it seems to work well for both. Note that your device has to be one of the devices that Vuforia supports for Device Tracking for the game to work as intended. 

# How To Play
Download and print Vuforia's Mars image trackers. The Astronaut tracker forms the base of the terrain. The Drone one is a "special" mirror, and the other two are normal mirrors.

Use the mirrors to direct the laser around to hit it at the ghosts. Sometimes a laser will be high up, so you need to raise it up with a physical object. Other times, you'll need more than one laser to get around a wall. Some ghosts are faster than others, and others require the special laser to take significant damage. All of these features add up to give a sampler of what is possible in the LazAR framework, though it could certainly be refined given more time.

While there are three difficulties, the aim of this project wasn't really to make three good, winnable levels. To be honest, medium and hard are unbeatable even if the terrain wasn't a little jittery, and part of this was intentional so that we could easily test that "Game Over" was working correctly.

On the topic of the jitteriness, that is something we spent way too much time trying to fix. What is currently available in the master branch should at least make the game somewhat playable, though is a bit of a hack. It basically tries to align the mirror's up vector with the terrain's if the discrepancy is small enough, and it does something similar with the height. Apparently, the version in master _might_ be causing the game to say you won almost immediately on certain iOS builds, but it does not do this for Android and it's not at all clear what the OS would have to do with this game feature. We have a smoothed average alternative to this that seems promising, but also requires further development.

Due to the time spent on the above (and the many failed prior attempts), we could not fix some of the other frustrations caused by extended tracking, such as objects being kept in place even when their cards have moved, odd behaviour for mirrors out of view, etc. The former of these two examples feels like the more complicated one. The latter has a theoretical solution we came up with that involves parenting a card's object to the terrain mesh whenever tracking on that card is lost, but we did not have time to actualize this in the code.

In spite of the above issues, we spent a great deal of effort trying to ensure that the main, key interactions still work, and in this we did succeed (at least, on our own devices and in our own lighting setups and whatnot), so hopefully you have the same experience!