# HMx Core
HMx Core consists of a range of utility code that we have found to be useful in the past across a number of different internal projects or client engagements. Although only open sourced in 2018 much of the code is considerably older. As such there are things in here that have been superseded by either updates to the .NET framework itself (such as much of the threading code since the advent of the TPL) or common adoption of other open source libraries.

Having said all that, we still find it useful. Broadly it consists of the following:

+ Config
    + Config utilities. This is particularly useful if targeting .NET Core or Mono to run on Linux and wanting to use linux style (Key=value) configuration files to keep things consistent with other parts of your system on Linux
+ DateTime
    + Date/Time utility functions and a common interface for providing the time (necessary for making any kind of time based code testable).
+ HTML
    + A parser for Dreamweaver template markup 
+ IO
    + Some utility file and directory functions
    + A parser for the Dropbox info file to locate the Dropbox folder on a user’s machine
+ Log
    + A common logging façade and some very simple implementations. We use this however because its written with a mind to be able to write tests that also check for the presence or absence of certain types of logging. Yes, we’re that OCD about our tests sometimes. As such there is “in memory” logger implementation which makes little sense from a production use perspective but is handy for testing. For real production use you’d likely want a log4Net implementation.
+ Net
    + Some email code which is largely just wrappers and standardised config and utility methods over the .NET framework
    + Socket/TCP code that handles things like framing the data over the wire (length prefix protocol provided, other protocols can be used), keep alive messages and dropped connection detection.
+ Serialization
    + Serialization code to help provide standardised serialization interfaces. Again, this is also written with a view to make code more testable too.
+ Threading
    + Mostly just implementations of the IAsyncResult interface that also deal with processing the work on another thread. As mentioned above though this is largely superseded by the TPL at this point and probably shouldn’t be used.

## Documentation
Much of the code is documented inline and the XML documentation files are included as part of the downloads. We are looking into hosting MSDN style documentation online but this is not yet available.

There is a decent amount of test coverage which in some regards can also serve as example code too.

## Downloads
All versions are available either packaged as a NuGet package from [NuGet](http://nuget.org) or as a zip artefact from the [HMx Labs](http://hmxlabs.uk/software) website

## License
HMx Labs Core is Open Source software and is licensed under an MIT style license. This allows use in both free and commercial applications without restriction. The [complete licence text](http://hmxlabs.uk/software/license.html) is available on the [HMx Labs website](http://hmxlabs.uk) or [here](./license.txt)

## Contributing, Bug Reports and Support
If you’ve found a bug or thought of a new feature that would be useful feel free to either implement it and raise a pull request for us or if you’d like us to take a look then just get in touch.

Support is provided very much on a best effort only basis and by electronic means (emails, pull requests, bug reports etc) only.

Unless you’re a client of HMx Labs of course, in which case just give us a call!


