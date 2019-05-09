using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileAddress {
    [SerializeField] private string name;
    [SerializeField] private string filePass;

    public string Name {
        get { return name; }
    }

    public string FilePass {
        get { return filePass; }
    }

    public FileAddress(string Name, string FilePass) {
        name = Name;
        filePass = FilePass;
    }
}
