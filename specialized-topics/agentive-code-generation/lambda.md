## Instructions to create an app using Agentive AI (.NET C#) (Single Project, Separate Utility Files, SOLID Compliant)

Goal: Build a console application that reads up to 10 numbers (terminated early by entering -1) and outputs each number along with its square and cube. The design must follow SOLID principles and allow easy extension with new mathematical operations via additional provider classes.

1. Initialize (or overwrite) a console project in the current directory using the standard .NET console template.

2. Confirm that the project contains a Program file. Keep it minimal (an empty Main entry point is fine) until after the interface and provider classes are created to avoid unresolved reference errors.

3. Use a single root namespace named Lambda for all source files. Place utility/provider classes in a Functions sub-namespace by storing them inside a folder named Functions.

4. Create a folder named Functions at the project root to hold transformation provider abstractions and their implementations.

5. Inside the Functions folder, define an interface named IFunctionProvider that exposes: 
   - A read-only string property representing a human-friendly name of the function (Name). 
   - A method that returns a delegate taking a double and returning a double (think: transformation function). 
   This interface is the abstraction the main program will depend on.

6. Still inside the Functions folder, create two separate classes implementing the interface: 
   - SquareFunctionProvider: returns a delegate computing the square of its input. 
   - CubeFunctionProvider: returns a delegate computing the cube of its input. 
   Each class should do only one thing (single responsibility) and should not perform I/O.

7. After the interface and the two provider classes exist and are saved, update the Program file to: 
   - Add the necessary using directives for core collections and for the Functions namespace. 
   - Allocate a collection to store up to 10 user-entered real numbers, stopping early if the user enters the sentinel value -1 (do not store the sentinel). 
   - Re-prompt on invalid numeric input without incrementing the stored count. 
   - Create a list of the provider interface type, adding both provider instances. 
   - Build a list of transformation delegates by invoking the retrieval method on each provider. 
   - For each entered number, compute and display: original value, its square, and its cube. Align the numeric columns for readability (for example, by using consistent width formatting or padding). 
   - If the user enters no values before the sentinel, display a short exit message.

8. Ensure the output is readable and columns are vertically aligned (e.g., right-aligned numbers of uniform width for original, square, and cube). No additional decoration is required.

9. Add or update a .gitignore file in the project root so that build artifacts, IDE folders, user-specific files, logs, and temporary files are excluded. Minimum entries: build output directories, IDE metadata, user preference files, log files, and temporary files.

10. Build then run the application (save all files first) using either the command line or the IDE build/run actions.

11. Troubleshooting guidance: 
    - If the interface type is reported as missing, verify the interface file name, namespace consistency with the rest of the project, presence of the Functions folder, and the correct using directive in the Program file. 
    - Always address the very first compiler error; subsequent errors often cascade from it. 
    - Confirm that all new files are included in the project (for SDK-style .NET projects this occurs automatically when files are placed under the directory structure).

(End of Instructions)