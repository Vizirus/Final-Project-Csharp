# MasterApplication
<h1>Welcome to the word counter application!</h1>
<h3>This application uses three processes: Agent, Agent2 and Master</h3>
<h3>Master works with agents by pipes and send path for each of agetns to parse files and look for some words</h3>
<h3>Agents send the result string by pipes back to master</h3>
<hr>
<h2>Application structure</h2>
<h3>DefaultAgent</h3>
<h4>Processes files for counting. Return final dictionary with counts of words for all files. Also returns count of words for some file</h4>

<h3>Agent Test</h3>
<h4>Contains 8 tests for DefaultAgent</h4>
<h4>Test1</h4>
<h4>TestOneMoreFile</h4>
<h4>DynamicChange</h4>
<h4>ReadFromLorenIpsum</h4>
<h4>ReadZero</h4>
<h4>AccessWrongToken</h4>

<h3>FileAgent and FileAgent_2</h3>
<h4>Contains implementation for DefaultAgent. Implements Named pipes to get the instructions for counting and to send results back</h4>

<h3>FinalProject</h3>
<h4>Contains two windows. Main point of application. Starts two agents, communicates with tem by pipes. Contains window for creating instructions for FileAgent and FileAgent_2</h4>
<hr>
<h2>History of commits</h2>
<h3>"Add .gitattribute, .gitignore and README.md" and Add project files</h3>
<h4>Initializing commits. Were made to upload basic structure to the github</h4>
<h3>UI addition, slight changes in the structure</h3>
<h4>Implemented starting version of UI in MAinWindow.xaml file</h4>
<h3>Merge pull</h3>
<h4>Slight unexpected action. Changes in the MainWindow.xaml.xs background code were made in second branch, so two branches were merged</h4>
<h3>Addition to the structure. Added Agent CL, Command signatrue and second while agent.</h3>
<h4>Added new files. Agent: Agent.csproj, DefaultAgent.cs, Instruction.cs</h4>
<h4>Added new files: FikeAgent_2: FileGaent_2.csproj. Program.cs</h4>

<h3>Added Agent behaivour and test</h3>
<h4>Implemented parsing and searching functionality, deleted instruction.cs file</h4>
<h4>Added 8 tests for agent for different cases</h4>

<h3>Added First working agent, fixed bugs, added new  UI elements for context editing</h3>
<h4>Added overloaded constructor for DefaultAgent</h4>
<h4>File project got basic implementation. Both agents implement Default Agents alognside with pipes</h4>
<h4>Added AddContextToPipe.xaml.cs UI window for changing data for parsing.</h4>
<h4>User is able to add multiple words and file for parsing and counting</h4>

<h3>Fixed bugs, implementing remova of some settings</h3>
<h4>Default Agent got new function. ReturnCount(string search word) for counting accurance of searched word in some file</h4>
<h4>FileAgent got bug fixed. Added behaivour for  ReturnCount(string search word) of DefaultAgent</h4>
<h4>Extending functionality of AddContextToPipes.xaml. Ability to delete words for search and to delete files to count in</h4>

<h3>Fixed bugs with two agents. Added agent B.</h3>
<h4>Bugs fixed in Agent A. DefaultAgent implemented for FileAgent_2 (AgentB) </h4>
<h4>UI changes for Agent B. All buttons gained functionality</h4>

<h3>Added save/open functions.</h3>
<h4>Added funcions for saving and opening</h4>
<h4>Save agent A... - saves text in the field of Agent A</h4>
<h4>Save agent B... - saves text in the field of Agent B</h4>
<h4>Save Both agents as... - saves text in both agents</h4>
<h4>Open for scanner A as... - opens text for the Agent A </h4>
<h4>Open for scanner B as... - opens text for the Agent B </h4>
