# BpCop

BpCop is a static code analysis module for Blue Prism processes and objects.

## Key facts

BpCop is

- easy to use - command line based analysis
- integrated - VBO based analysis
- intermateable - all results are available as ascii tables, markdown, html or json
- flexible - can deal with Blue Prism databases, exported assets, release files or with the AutomateC directly
- configurable - rules can be black and white listed
- extensible - the solution is based on .NET MEF and easy to extend e.g. with customer own written rules. Rules can be loaded from different locations
- sustainable - a finding can be justified by a note (globally, page or stage based)
- approved - this module is used for a while
- complete - more than 60 rules (devided into sets for programming, design and security) found by community research and our own experience (and growing)
- future-proof - the core is written in .NET Standard 2.0 and fits to classic .NET Framework and new .NET 6+
- fast - checking 60 rules at a process or VBO is done in round about 1-2 seconds
- simple - common parameters can be arranged in profiles
- portable - no installation is required

## Basic usage

BpCop uses verbs to specify a command. Supported verbs are `check-automate, check-file, check-db, check-named-db, show-rules`.
Each verb supports different switches. A help page is available for each verb.

Example (Test a .bprelease file)
`BpCop.Console.exe check-file -f "D:\BluePrism\BpCop\BpCop - Tests.bprelease"`.
This checks all assets in the release file.

Example (Test only the assets 'SC008 - Test' and 'Page Justification - Test') from the mentioned file
`BpCop.Console.exe check-file -f "D:\BluePrism\BpCop\BpCop - Tests.bprelease" -a "SC008 - Test" "Page Justification - Test"`

Example (Show all available rules)
`BpCop.Console.exe show-rules`

## Justifications
The `show-rules` verb displays if and how a finding can be justified.
Justifications can be applied usually by one or multiple Blue Prism note(s) on the corresponding page.
A justification is a text matching the template
`[RuleName;StageName;Justification text]`
`StageName` is only needed by stage-level justifications.
Multiple justifications can be applied in one note.
It is good practice to add a justification date and the name of the approver to the justification text.

|Justification level|Description|Example|
|-------------------|-----------|-------|
|Page|Justify a rule the the hole page| [DS001 Design needs more than 20 stages] |
|Global|Justify a rule for the whole asset. For the sake of readability put this justifications on the `Main`or `Initialise` page (Note must contain the text 'global' in its name)|[PG004 Unconditional locks are needed here]|
|Stage|Justify a rule only for a single stage. Refer to the stage by stage name embedded in semicolons `;`| [PG009;Throw Exception;No exception logging is needed here]|
|AppModel|Due to missing notes in the AppModel justifications should be entered in the `Description` field|[DS025 TA 08/06/2023 disabled due to demo purposes]|
|None|These are serious errors which are not justifiable| -- |