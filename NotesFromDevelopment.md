# Terrain
Unity's terrain apparently does not play nice with Vuforia.

However, Unity's terrain tool is really nice; or, at least, the open-source Path Paint Tool modifier for it is very nice to create a mountain with a "path", which is what our game is centered around at the time of this writing.

The solution:

 1. Make a terrain at least 100x100 units in size (note: this is different than the _resolution_); it needs to be about this large in order for the path paint tool to work.
 2. Create the terrain and edit it using the path paint tool.
 3. Export the terrain, using 8-bit encoding (GIMP cannot handle 16 bit or something), and use the file extension .data instead of the default .raw (again, so that Gimp can handle it).
 4. Import it in GIMP; make sure the resolution matches the heightmap resolution in Unity, and make sure that you use the Indexed image type.
 5. Export as .png
 6. Create a heightmap in Blender using this heightmap texture with the Displace modifier. Make sure to use smooth shading.
 7. Export from Blender and use the height map in the scene.

# Building and Terrain Tools
I found that the Terrain Tools were somehow failing the build, even though they were not actually used in the finished build; they're just in the asset tool for the sake of convenience if we want to modify the terrain used or add a new one.

To fix this, I added a "~" to the end of the directory name. This apparently excludes the assets from the build process in newer versions of Unity.

