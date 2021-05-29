# stealing-my-friends-files
This was desktop program which I wrote a long ago to steal my friends files from pen-drive (flash drive). This program:
1. runs in background;
2. monitors if any pen-drive in plugged in to the computer; 

Whenever plugged-in, it will start two threads. - 

3. one will look for my predefined types of files; and another thread will be busy in copying those files.
4. if any error occurred, writes error log in a text file. When logging, mutex is used since two thread may try to write in the log file simultaneously.
