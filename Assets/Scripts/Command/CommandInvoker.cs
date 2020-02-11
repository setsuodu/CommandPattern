﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandBuffer;

    static List<ICommand> commandHistory;
    static int counter;

    private static string logRoot;
    private static string LogRoot
    {
        get 
        {
            if (string.IsNullOrEmpty(logRoot))
                logRoot = Path.Combine(Application.dataPath, "Logs");

            if (!Directory.Exists(logRoot))
                Directory.CreateDirectory(logRoot);

            return logRoot;
        }
    }

    void Awake()
    {
        commandBuffer = new Queue<ICommand>();
        commandHistory = new List<ICommand>();
    }

    void Update()
    {
        if (commandBuffer.Count > 0)
        {
            ICommand c = commandBuffer.Dequeue();
            c.Execute();
            //commandBuffer.Dequeue().Execute();

            // 回收历史记录
            commandHistory.Add(c);
            counter++;
            Debug.Log("Command histroy length: " + commandHistory.Count);
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (counter > 0)
                {
                    counter--;
                    commandHistory[counter].Undo();
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (counter < commandHistory.Count) 
                {
                    commandHistory[counter].Execute();
                    counter++;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExportLog();
        }
    }

    public static void AddCommand(ICommand command)
    {
        while (commandHistory.Count > counter)
        {
            commandHistory.RemoveAt(counter);
        }
        commandBuffer.Enqueue(command);
    }

    static void ExportLog() 
    {
        List<string> lines = new List<string>();
        foreach (ICommand c in commandHistory) 
        {
            lines.Add(c.ToString());
        }

        string fileName = "commandlog.txt";
        string filePath = Path.Combine(LogRoot, fileName);
        File.WriteAllLines(filePath, lines);
    }
}