# templeEscape
**this whole read me file is temp and will most likely change if anyone wants it to - to make any changes make a seperate branch where only the readme file is touched and do a merge request**

# game description - not finished
a game where the player goes around from room to room finding pieces while avoiding the enemy.

# how the repo will function
- every group creates their own team branch
    - it is up to each group how they orginise there own branch

- when ever a feature is complete, like the design of a puzzle piece is complete or the intro maze design is finished, then make a merge request to main

- make sure your teams branch is always up to date with the main branch

- if you make a merge request to main branch send a message in the teams chat so everyone knows and is ready to update their own branches

# how to use github
## getting the repo onto your computers
## if Git Bash Here avaliable
1. find the URL to the repo - should look exactly like the image

![repo url image](/ReadMeImages/RepoUrl.jpg)

2. go into the folder you wish to insert the repo

3. open Git Bash Here

![git bash image](/ReadMeImages/GitBashOpening.jpg)

4. enter **git clone repoURL**

![clone code](/ReadMeImages/CloneCode.jpg)

5. wait for it to download and then open the repo and do what you want


## **_saving to github - ask person in team with most knowledge with github before continuing incase they want it done a different way_**

1. create a .gitignore inside the repo since you wont be able to push everything since its too large [(how to create a .gitignore)](#creating-a-gitignore)

## if doing it by **Git Bash Here**

2. open git bash inside the repo

3. create a new branch by entering - make a main branch for each team and decide per group how you will orginise it - only do if you need a new branch, not needed
``` 
git chechout -b new-branch-name 
```

4. enter into git bash here - only if the branch is newly created and has nothing on it
``` 
git push --set-upstream origin new-branch-name
```

5. enter into **git bash here** - make sure the ignore file is complete before continuing in steps or wont work
``` 
git add .   
```

6. enter into **git bash here** 
``` 
git commit -m "your commit message. make sure its detailed"
```

8. make sure you are trying to save to right branch - to check it will say above the line you typing (its the same code as step 3 except without the -b and you say the name of the currently existing branch)

7. enter into **git bash here** 
``` 
git push
```

8. see if saved on github

will expand on this at a later date when merging branches is needed or other important things

## **_creating a gitignore_**

## how to start - only way i know please update if know other / better ways

1. download the github desktop app

2. select the repo you have been working in

3. open the repository selection tool at top left when in repo

4. go to repository settings

5. select ignored files

6. enter the following code - be aware there is problems where XR management package gets uninstalled and you will have to reinstal it. working on better code
```
# This .gitignore file should be placed at the root of your Unity project directory
#
# Get latest from https://github.com/github/gitignore/blob/main/Unity.gitignore
#
/[Ll]ibrary/
/[Tt]emp/
/[Oo]bj/
/[Bb]uild/
/[Bb]uilds/
/[Ll]ogs/
/[Uu]ser[Ss]ettings/

# MemoryCaptures can get excessive in size.
# They also could contain extremely sensitive data
/[Mm]emoryCaptures/

# Recordings can get excessive in size
/[Rr]ecordings/

# Uncomment this line if you wish to ignore the asset store tools plugin
# /[Aa]ssets/AssetStoreTools*

# Autogenerated Jetbrains Rider plugin
/[Aa]ssets/Plugins/Editor/JetBrains*

# Visual Studio cache directory
.vs/

# Gradle cache directory
.gradle/

# Autogenerated VS/MD/Consulo solution and project files
ExportedObj/
.consulo/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd
*.pdb
*.mdb
*.opendb
*.VC.db

# Unity3D generated meta files
*.pidb.meta
*.pdb.meta
*.mdb.meta

# Unity3D generated file on crash reports
sysinfo.txt

# Builds
*.apk
*.aab
*.unitypackage
*.app

# Crashlytics generated file
crashlytics-build.properties

# Packed Addressables
/[Aa]ssets/[Aa]ddressable[Aa]ssets[Dd]ata/*/*.bin*

# Temporary auto-generated Android Assets
/[Aa]ssets/[Ss]treamingAssets/aa.meta
/[Aa]ssets/[Ss]treamingAssets/aa/*
```
