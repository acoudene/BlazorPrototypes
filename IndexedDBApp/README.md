# Objective

Test IndexedDB through Blazor and a dedicated component for interop with Javascript API.

# Example

![image](https://github.com/user-attachments/assets/3c1da65c-bc5a-44f1-9a2a-8520e55d3aa0)

# Constraints

- Be careful about keeping IndexedDB persistant, don't remove it after closing browser like this:

![image](https://github.com/user-attachments/assets/69f3f0e8-6dcf-43af-8cb4-e73bd1963e87)

- Use this fork [Blazor.IndexedDB](https://github.com/acoudene/Blazor.IndexedDB)

# Limitations

 - Quite slow to read data from IndexedDB.
 - Be careful on default limitations on file reading in C# (max size and max number of multiple files).
