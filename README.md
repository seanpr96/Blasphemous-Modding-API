Building The API 
============================
Building the API is fairly straightforward.

1. Clone this!
2. Go to `%BlasphemousGameInstallPath%/Blasphemous_Data/Managed/` and copy it's contents to the `Vanilla` folder in this repository. (Create the Vanilla folder if it does not exist.)
3. Open the solution in Visual Studio 2017 or Rider (You can also just use msbuild/xbuild)
4. Set the build configuration to Debug and build.
5. The patched assembly should be in `%RepoPath%/Output/`
6. Copy the files in this folder to `%BlasphemousGameInstallPath%/Blasphemous_Data/Managed/`