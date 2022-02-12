# automatic-file-copier
This is a desktop program to automatically copy files from pen-drive (flash drive); which I copy-pasted in a .netcore-winform application to uploaded here in Github. 

This program: 
* runs in background.
* monitors if any pen-drive plugged-in into the computer.

Whenever plugged-in, it will start two threads-
* thread one: will start searching entire flash-drive for predefined types of files (video-audio/code/compressed), and will add them into a queue.
* thread two: will be busy in copying files from the queue.

If any error occurred in any thread, they write log in a text file. While writing, mutex is used, since two threads may try to write simultaneously.

*Developed on Jan 2013*
