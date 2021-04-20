#
# Script: CopyFilesToServer.ps1
# Written By: Bob Elward Apr 2021
#
# Description:
# Uses PuTTY utilitiy PSCP to copy files from Windows to a Linux server.
#
# To Execute from DOS Comandline:
# powershell ./CopyFilesToServer.ps1       <=== Note the "./" is required
#
#==============================================================================
#
""
"Executing: CopyFilesToServer.ps1"
#
# Control variable:
#
$command = "pscp"
$identity = "-i C:\Users\Bobel\.ssh\puttyutils.ppk"
$port = "-P 22"
#
$projectdir = "CSC543FinalProject/"

#
# C# source code
#
$subdir = $projectdir
$files = $subdir + "*.cs"
$to = "relwa136@kuvapcsitrd01.kutztown.edu:/home/students.kutztown.edu/relwa136/csc543/csharp/" + $projectdir
#
"Project Directory: " + $projectdir
#
# Command arguments
#
$arguments = $identity + " " + $port +" " + $files + " " + $to
"arguments: " + $arguments
#
# Run command
#
Start-Process -NoNewWindow `
              -Wait `
              -FilePath $command `
              -ArgumentList $arguments
			  
#
# text file
#
$subdir = $projectdir
$files = $subdir + "*.txt"
$to = "relwa136@kuvapcsitrd01.kutztown.edu:/home/students.kutztown.edu/relwa136/csc543/csharp/" + $projectdir
#
"Project Directory: " + $projectdir
#
# Command arguments
#
$arguments = $identity + " " + $port +" " + $files + " " + $to
"arguments: " + $arguments
#
# Run command
#
Start-Process -NoNewWindow `
              -Wait `
              -FilePath $command `
              -ArgumentList $arguments
			  
#
# C# project file
#
$subdir = $projectdir
$files = $subdir + "*.csproj"
$to = "relwa136@kuvapcsitrd01.kutztown.edu:/home/students.kutztown.edu/relwa136/csc543/csharp/" + $projectdir
#
"Project Directory: " + $projectdir
#
# Command arguments
#
$arguments = $identity + " " + $port +" " + $files + " " + $to
"arguments: " + $arguments
#
# Run command
#
Start-Process -NoNewWindow `
              -Wait `
              -FilePath $command `
              -ArgumentList $arguments
			  
#
# PowerPoint file
#
$subdir = $projectdir
$files = $subdir + "*.pptx"
$to = "relwa136@kuvapcsitrd01.kutztown.edu:/home/students.kutztown.edu/relwa136/csc543/csharp/" + $projectdir
#
"Project Directory: " + $projectdir
#
# Command arguments
#
$arguments = $identity + " " + $port +" " + $files + " " + $to
"arguments: " + $arguments
#
# Run command
#
Start-Process -NoNewWindow `
              -Wait `
              -FilePath $command `
              -ArgumentList $arguments
			  
#
# POWERSHELL scripts
#
$subdir = "./"
$files = $subdir + "*.ps1"
$to = "relwa136@kuvapcsitrd01.kutztown.edu:/home/students.kutztown.edu/relwa136/csc543/csharp/" + $projectdir
#
"Project Directory: " + $projectdir
#
# Command arguments
#
$arguments = $identity + " " + $port +" " + $files + " " + $to
"arguments: " + $arguments
#
# Run command
#
Start-Process -NoNewWindow `
              -Wait `
              -FilePath $command `
              -ArgumentList $arguments
