# qbuild
======

Command-line tool to queue TFS builds through a build definition.

http://rogah.github.io/qbuild/

### Dependencies:
Microsoft .NET Framework 4.5.1

### Environment:
Set you environment variable *PATH* to point to the folder where you place qbuild.

### Parameters:
```Batchfile
qbuild -c <collection> -p<project> -d <definition> -v <version> [-i <interval>] [-t <timeout>] [-x] --<parameter_name> <parameter_value> [--<parameter_name> <parameter_value> [...]]]

        c:collection            Collection url (e.g. https://tsf.dell.com:8080/tfs/dfs).
        d:definition            Build definition name (e.g. ProjectName-Relese#).
        i:interval              Polling interval in seconds. Default is 5.
        p:project               Project name (e.g. Project-Name).
        t:timeout               Timeout in minutes. Default is 30.
        v:version               Build version (e.g. ProjectName-20140110.1).
        x:verbose               Verbose logs. Default is false.
```

### Return Codes:
* Success = 0
* Failed = 1
* PartiallySucceeded = 2
* NotStarted = 3
* Stopped = 4
* ParseFailed = 5
* None = 10

Sample:
```Batchfile
qbuild -c http://tfs.dell.com:8080/tfs/DFS -p DFS-SFDC -d FC-R1.3-CI -v FC-R1.3_20140915.5 --SfUsername foo@bar.com
```

