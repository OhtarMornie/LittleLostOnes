using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using AC;

public class ImportSpeechEditor : EditorWindow
{

    private Cutscene cutsceneToOverwrite;


    [MenuItem("Adventure Creator/Addons/Import speech lines", false, 10)]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        ImportSpeechEditor window = (ImportSpeechEditor)EditorWindow.GetWindow(typeof(ImportSpeechEditor));
        window.titleContent.text = "Speech importer";
    }


    private void OnGUI()
    {
        cutsceneToOverwrite = (Cutscene)EditorGUILayout.ObjectField("Cutscene to overwrite:", cutsceneToOverwrite, typeof(Cutscene), true);

        if (GUILayout.Button("Import from file"))
        {
            ImportFromFile();
        }
    }


    private void ImportFromFile()
    {
        string fileName = EditorUtility.OpenFilePanel("Import inventory item data", "Assets", "csv");
        if (fileName.Length == 0)
        {
            return;
        }

        if (System.IO.File.Exists(fileName))
        {
            string csvText = Serializer.LoadFile(fileName);
            string[,] csvOutput = CSVReader.SplitCsvGrid(csvText);

            GenerateLines(csvOutput);
        }
    }


    private void GenerateLines(string[,] csvData)
    {
        int numCols = csvData.GetLength(0) - 1;

        if (numCols < 3) return;

        int numRows = csvData.GetLength(1);

        List<ImportedSpeechLine> importedSpeechLines = new List<ImportedSpeechLine>();

        for (int row = 1; row < numRows; row++)
        {
            if (csvData[0, row] != null && csvData[0, row].Length > 0)
            {
                int isPlayerInt = 0;
                if (int.TryParse(csvData[0, row], out isPlayerInt))
                {
                    bool isPlayer = (isPlayerInt == 1);
                    string characterName = csvData[1, row];
                    string lineText = csvData[2, row];

                    importedSpeechLines.Add(new ImportedSpeechLine(isPlayer, characterName, lineText));
                }
            }
        }

        ImportLines(importedSpeechLines.ToArray());
    }


    private void ImportLines(ImportedSpeechLine[] importedSpeechLines)
    {
        if (cutsceneToOverwrite != null)
        {
            cutsceneToOverwrite.actions.Clear();

            foreach (ImportedSpeechLine importedSpeechLine in importedSpeechLines)
            {
                ActionSpeech speechAction = importedSpeechLine.GenerateAction();
                cutsceneToOverwrite.actions.Add(speechAction);

            }

            // Save
            UnityVersionHandler.CustomSetDirty(cutsceneToOverwrite, true);
        }
    }


    private struct ImportedSpeechLine
    {

        private string text;
        private bool isPlayer;
        private string speakerName;


        public ImportedSpeechLine(bool isPlayer, string speakerName, string text)
        {
            this.isPlayer = isPlayer;
            this.speakerName = speakerName;
            this.text = text;
        }


        public ActionSpeech GenerateAction()
        {
            if (isPlayer)
            {
                return ActionSpeech.CreateNew_Player(text);
            }
            else
            {
                AC.Char speaker = null;
                if (!string.IsNullOrEmpty(speakerName))
                {
                    AC.Char[] allCharacters = GameObject.FindObjectsOfType<AC.Char>();
                    foreach (AC.Char character in allCharacters)
                    {
                        if (character.GetName() == speakerName)
                        {
                            speaker = character;
                            break;
                        }
                    }
                }

                return ActionSpeech.CreateNew(speaker, text);
            }
        }

    }

}