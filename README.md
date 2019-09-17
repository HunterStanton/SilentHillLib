# SilentHillLib

A C# library for manipulating data in the Silent Hill series (specifically, Silent Hill 2-4, with support for more games potentially coming, if there is interest).

## Project Goals
The main goal of SilentHillLib is to provide a standard interface for viewing and manipulating data in the Silent Hill games. It aims to abstract away all the technical bits and leave you with just the data that you're after. For example, extracting a DDS texture from SH4 can be done with 4 lines of code. This makes it super simple to build a tool like a texture viewer/importer, because you don't need to reverse-engineer the .bin file format that SH4 uses to store it's textures.

One of the biggest goals of the project is modding. The Silent Hill series is one that, in my opinion, is rife with modding opportunities, but the tools simply don't exist for that. The few tools that do exist for modding only deal with textures or text, and generally have limits (such as being unable to resize textures). SilentHillLib aims to break all of those barriers and provide the ability for a developer to write a tool that is capable of importing anything into the SH games, be that a texture, an animation, or even a whole new game world.

## Compatibility
|  Data Type |  Silent Hill 2 |  Silent Hill 3 | Silent Hill 4 |
|------------|----------------|----------------|---------------|
|  Models    | NO             | NO             | NO            |
| Textures   | NO             | NO             | YES           |
| Animations | NO             | NO             | NO            |

At the moment, SilentHillLib is very rudimentary and does not handle anything but exporting SH4 textures. The API is in a state of flux at the moment and will not be finalized for quite some time, so be prepared to make changes if you want to build something using SilentHillLib until it gets to a release worthy state.

That being said, feel free to pull the latest code down and try it out.

## Credits
* Hunter Stanton (@HunterStanton) - Reverse-engineering, programming
* roocker666 - Reverse-engineering
