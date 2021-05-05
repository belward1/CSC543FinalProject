Contents of the Zip File:

Note: C# is pronounced "C Sharp"

1) This readme.txt file 
2) C# Source code: Files that end with *.cs
3) C# Project file: File that ends with *.csproj
4) makefile  (For compiling and running on Linux)
5) PowerPoint Slide deck: File ending in *.pptx
6) PowerPoint Slide deck as PDF: File ending in *.pdf
7) Visio file of the MapReduce process: File ending in *.vsdx
8) Large text file for MapReduce: world192.txt
9) PowerShell command file I use to copy from my Windows PC to KU Linux servers: File ending in *.ps1


======================================================================================================

Prerequisites to point to the .NET runtime on ACAD(CSITRD) or McGonagal:

The .NET runtime is installed on the ACAD(CSITRD)/McGonagal file system under my account (/home/students.kutztown.edu/relwa136) in the directory ".dotnet".  

I have given the following permissions to this directory:

drwxrwxr-x.   10 relwa136 relwa136       4096 Jan 23 14:26 .dotnet



I had to add the following lines to my .bash_profile:

# User specific environment and startup programs

PATH=$PATH:$HOME/bin:$HOME/.dotnet

#export PATH
export DOTNET_ROOT=$HOME/.dotnet


I'm going to guess yopu will need something like:

# User specific environment and startup programs

PATH=$PATH:$HOME/bin:/home/students.kutztown.edu/relwa136/.dotnet

#export PATH
export DOTNET_ROOT=/home/students.kutztown.edu/relwa136/.dotnet


You may need to logoff and log back in for these commands to execute.


======================================================================================================

To test that this is working, execute the following command:

dotnet --version

and you should get back:

5.0.102


======================================================================================================

To compile and execute this project on Linux:

1) Unzip all the files to it's own dirctory, e.g., CSC543FinalProject
2) Navigate to the directory
3) Run: make run









