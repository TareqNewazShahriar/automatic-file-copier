# stealing-my-friends-files
This was desktop program which I wrote a long ago (on January 2013) to steal my friends files from pen-drive (flash drive); which I copy-pasted in a .netcore-winform application to uploaded here in Github. This program:
* runs in the background
* monitors if any pen-drive in plugged in to the computer

Whenever plugged-in, it will start two threads. -

* one will look for my predefined types of files (video-audio / code / compressed)
* another thread will be busy in copying those files
* if any error occurred, writes error log in a text file. When logging, mutex is used, since two threads may try to write simultaneously
