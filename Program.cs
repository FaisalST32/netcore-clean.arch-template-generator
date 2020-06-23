using System;
using System.IO;
using System.Linq;

namespace netcore.cleanarch
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args == null || args.Length != 2)
      {
        Console.WriteLine("Incorrect Syntax");
        Console.WriteLine("Usage: cleanarch <SolutionName> <ProjectPrefix>");
        Console.WriteLine("e.g., cleanarch FaisalsBlog Blog");
        return;
      }
      string solutionName = args[0];
      string projectPrefix = args[1];
      string appDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

      Console.WriteLine("Project is installed in " + appDirectory);
      Console.WriteLine("Current Directory " + Directory.GetCurrentDirectory());

      Console.WriteLine("Creating Solution Directory");
      Console.WriteLine("Moving Project Files");
      MoveFiles(solutionName);
      Console.WriteLine("Transforming Files");
      ReplaceContent(solutionName, projectPrefix);
      Console.WriteLine("Renaming Directories");
      RenameFolders(solutionName, projectPrefix);
      Console.WriteLine("Renaming Files");
      RenameFiles(solutionName, projectPrefix);

      //   var files = Directory.GetFiles("C:\\Faisal\\CodingWork\\templatefiles - Copy", "*.*", SearchOption.AllDirectories);
      //   foreach (string file in files)
      //   {
      //       Console.WriteLine(file);
      //       File.Move(file, file + ".file");
      //   }
    }

    static void MoveFiles(string solutionName)
    {
      string currentDirectory = Directory.GetCurrentDirectory();
      string solutionDirectory = currentDirectory + $"\\{solutionName}";
      Directory.CreateDirectory(solutionDirectory);
      string applicationDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
      string templateFilesDirectory = applicationDirectory + "\\templatefiles";

      foreach (string dirPath in Directory.GetDirectories(templateFilesDirectory, "*",
          SearchOption.AllDirectories))
        Directory.CreateDirectory(dirPath.Replace(templateFilesDirectory, solutionDirectory));

      foreach (string newPath in Directory.GetFiles(templateFilesDirectory, "*.*",
          SearchOption.AllDirectories))
        File.Copy(newPath, newPath.Replace(templateFilesDirectory, solutionDirectory), true);
    }

    static void ReplaceContent(string solutionName, string projectName)
    {
      string currentDirectory = Directory.GetCurrentDirectory();
      string solutionDirectory = currentDirectory + $"\\{solutionName}";
      var files = Directory.GetFiles(solutionDirectory, "*.*", SearchOption.AllDirectories);
      foreach (string file in files)
      {
        var text = File.ReadAllText(file);
        text = text.Replace("~ProjectPrefix~", projectName);
        text = text.Replace("~SolutionPrefix~", solutionName);
        File.WriteAllText(file, text);
      }
    }

    static void RenameFiles(string solutionName, string projectName)
    {
      string currentDirectory = Directory.GetCurrentDirectory();
      string solutionDirectory = currentDirectory + $"\\{solutionName}";
      var files = Directory.GetFiles(solutionDirectory, "*.*", SearchOption.AllDirectories);
      for (int i = 0; i < files.Length; i++)
      {
        try
        {
          if (files[i].Split("\\").Last().Contains("~ProjectPrefix~"))
          {
            File.Move(files[i], ChangePath(files[i], "~ProjectPrefix~", projectName));
            files[i] = ChangePath(files[i], "~ProjectPrefix~", projectName);
          }
          if (files[i].Split("\\").Last().Contains("~SolutionPrefix~"))
          {
            File.Move(files[i], ChangePath(files[i], "~SolutionPrefix~", solutionName));
            files[i] = ChangePath(files[i], "~SolutionPrefix~", solutionName);
          }
          File.Move(files[i], files[i].Replace(".file", ""));
        }
        catch
        {
          Console.WriteLine(files[i]);
          throw;
        }

      }
    }

    static void RenameFolders(string solutionName, string projectName)
    {
      string currentDirectory = Directory.GetCurrentDirectory();
      string solutionDirectory = currentDirectory + $"\\{solutionName}";
      var directories = Directory.GetDirectories(solutionDirectory, "*", SearchOption.AllDirectories);
      foreach (string directory in directories)
      {
        if (directory.Split("\\").Last().Contains("~ProjectPrefix~"))
        {
          Directory.Move(directory, ChangePath(directory, "~ProjectPrefix~", projectName));
        }
        if (directory.Split("\\").Last().Contains("~SolutionPrefix~"))
        {
          Directory.Move(directory, ChangePath(directory, "~SolutionPrefix~", solutionName));
        }
      }
    }

    static string ChangePath(string path, string stringToFind, string replacement)
    {
      var pathSplit = path.Split("\\");
      pathSplit[pathSplit.Length - 1] = pathSplit[pathSplit.Length - 1].Replace(stringToFind, replacement);
      return string.Join('\\', pathSplit);  
    }
  }
}
