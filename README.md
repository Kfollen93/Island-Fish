# Island Fish

<img src="https://github.com/Kfollen93/Island-Fish/blob/main/Images/fishing.gif" width="640" height="360"/>

You can download the game from my Itch.io page here: <a href="https://kfollen.itch.io/island-fish">Island Fish</a>. <br>

<i>*Due to the original (private) repository that I used throughout development containing paid assets, I have made this new repository public with only the code available within it. I've set up my game repositories this way to be able to share my code, without violating any asset copyright rules. The private repository containing all project files and commit messages may be available upon request. </i> <br>

## Description
Island Fish is a fishing collection prototype. There are eight different fish to be caught from multiple bodies of water, with each fish having its own unique stats. You must find bait worms along the coast in order to fish. The end goal is to catch every fish at its max size (trackable info in the player's journal).<br>

All progression can be saved and loaded (via JSON).

## Development
The overall goal I had envisioned for this project was to create a complete prototype/game. My prior projects were more focused on individual components: mobile with Rigidbodies, simple quest system with dialogue text, and more of the alike. With this project I wanted to include data in the form of Scriptable Objects, have a flow of starting with a cutscene, a main gameplay loop that could actually be completed, along with all the extra things that you would expect from a complete game such as a menu with system settings, an interactive UI, stats to track, and a saving and loading system.<br>

In the end I feel like I accomplished what I set out to do from the start, but that's not to say it's a perfect project.

## Learning Outcome
I planned a lot for this prototype with a [diagram board](https://www.diagrams.net/) for initial high-level scope and ideas, [Trello board](https://trello.com/) for programming tasks and overall progress, along with many empty projects for testing assets. Yet, even with all of this preparation, I still stumbled a bit and learned even more. <br>

The main component of this prototype was a fishing system, which I struggled with a lot. From several iterations of using a line renderer, to implementing my own version of a [Verlet Integration](https://en.wikipedia.org/wiki/Verlet_integration) fishing line, to eventually landing on purchasing [Obi Rope](https://assetstore.unity.com/packages/tools/physics/obi-rope-55579). Although Obi Rope is amazing for simulating rope physics, it is extremely complex. I spent nearly a month trying to understand how the asset worked and I lost a lot of time and motivation. I should have had a plan for how I was going to implement the fishing line, and stuck with it from the beginning. <br>

Beyond the fishing line, I did not spend much time thinking about how to add details to the fishing system. This was purely on me having conflicting interests. My main goal was to create a comprehensive project, including all of the aforementioned systems, yet I overlooked the details of the main feature. I had added the fish Scriptable Objects, implemented saving and loading, had a cutscene, spent time creating the island, made a shop system with camera switching, but the fishing system, although it certainly works, could have used more time and planning.

## End Thoughts & Future
I will shift how I think when it comes to implementing features. I found myself writing code for specific items/objects, rather than taking a bit more time to create a reusable system that is open for extension to apply to several similar objects. I really enjoyed creating custom Inspectors with [Odin](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041) and I gained a better understanding of how to work with Scriptable Objects.<br>
<br>
<img src="https://github.com/Kfollen93/Island-Fish/blob/main/Images/inspector.png" width="640" height="360"/><br>
<i>Fish Scriptable Object custom Inspector example.</i>

This project also highlighted some key programming areas that I still struggle with and made me realize I need to directly spend time studying and practicing, some of those areas are:

- Implementing Actions, Events, and Delegates more often.
- Moving logic out of the `Update()` loop into more sub/unsub systems.
- Getting a better handle on references to not rely on `FindWithTag()`.
- Decoupling in general.

I don't know what my next project will be at the moment, but I am certain it will be focused around my weak points mentioned above. I recently [built a template for Ray Tracing](https://github.com/Kfollen93/HDRP-Ray-Tracing) in Unity's HDRP, which I am currently interested in exploring more of in regards to lighting and Ray Tracing.
## Screenshots
<img src="https://github.com/Kfollen93/Island-Fish/blob/main/Images/caught.png" width="640" height="360"/>
<img src="https://github.com/Kfollen93/Island-Fish/blob/main/Images/sea.png" width="640" height="360"/>
<img src="https://github.com/Kfollen93/Island-Fish/blob/main/Images/bridge.png" width="640" height="360"/>
<img src="https://github.com/Kfollen93/Island-Fish/blob/main/Images/journal.png" width="640" height="360"/>

## Additional Information
<ul>
  <li>Made with: Unity 2020.3.26 (URP)</li>
</ul>
External Tools Used:
<ul>
  <li><a href="https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676">DOTween</li>
  <li><a href="https://assetstore.unity.com/packages/tools/animation/animancer-pro-116514">Animancer Pro</li>
  <li><a href="https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041">Odin Inspector</li>
  <li><a href="https://assetstore.unity.com/packages/vfx/shaders/low-poly-water-builtin-urp-poseidon-153826">Poseidon</li>
  <li><a href="https://assetstore.unity.com/packages/tools/physics/obi-rope-55579">Obi Rope</li>
</ul>
