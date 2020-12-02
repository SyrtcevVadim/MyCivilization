using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CreditsMenuLogic : MonoBehaviour
{
    public static Text projectLeader;
    public static Text producer;
    public static Text programmersLabel;
    public static Text softwareTestersLabel;
    public static Text firstProgrammer;
    public static Text secondProgrammer;
    public static Text thirdProgrammer;
    public static Text soundDesigner;
    public static Text firstSoftwareTester;
    public static Text secondSoftwareTester;
    public static Text thirdSoftwareTester;

    string specialCodeSequence = "OBL";
    string fetchedSequence = "";
    private void Awake()
    {
        projectLeader = GameObject.Find("ProjectLeaderLabel").GetComponent<Text>();
        producer = GameObject.Find("ProducerLabel").GetComponent<Text>();
        firstProgrammer = GameObject.Find("Programmer1Label").GetComponent<Text>();
        secondProgrammer = GameObject.Find("Programmer2Label").GetComponent<Text>();
        thirdProgrammer = GameObject.Find("Programmer3Label").GetComponent<Text>();
        soundDesigner = GameObject.Find("SoundDesignerLabel").GetComponent<Text>();
        firstSoftwareTester = GameObject.Find("Tester1Label").GetComponent<Text>();
        secondSoftwareTester = GameObject.Find("Tester2Label").GetComponent<Text>();
        thirdSoftwareTester = GameObject.Find("Tester3Label").GetComponent<Text>();
        programmersLabel = GameObject.Find("ProgrammersLabel").GetComponent<Text>();
        softwareTestersLabel = GameObject.Find("SoftwareTestersLabel").GetComponent<Text>();

        projectLeader.text = "Leader of the project: Syrtcev Vadim Igorevich";
        producer.text = "Producer: Syrtcev Vadim Igorevich";
        firstProgrammer.text = "Syrtcev Vadim Igorevich - Lead programmer";
        secondProgrammer.text = "Purojok - Cool guy";
        thirdProgrammer.text = "FirstPolyarnik - Hell machine";

        soundDesigner.text = "Sound designer: Syrtcev Vadim Igorevich";
        firstSoftwareTester.text = "Syrtcev Vadim Igorevich - incognito";
        secondSoftwareTester.text = "Rolark - chort and the author of fundamental algorithms(but still chort)";
        thirdSoftwareTester.text = "Trina - fun of the English language";
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            fetchedSequence += KeyCode.O.ToString();
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            fetchedSequence += KeyCode.B.ToString();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            fetchedSequence += KeyCode.L.ToString();
        }
        if(fetchedSequence == specialCodeSequence)
        {
            DisplaySpecialCreditsMenu();
            fetchedSequence = "";
        }
    }
    private void DisplaySpecialCreditsMenu()
    {
        projectLeader.fontSize = 18;
        projectLeader.text = "Purojok";
        producer.fontSize = 18;
        producer.text = "Trina";
        programmersLabel.fontSize = 18;
        programmersLabel.text = "Rolark - более известен, как ленивый чорт. Поддерживает и дает ценные советы по алгоритмам";
        firstProgrammer.fontSize = 18;
        firstProgrammer.text = "Forest_wayfarer - освобождает бюджетные места и ломает ребра";
        secondProgrammer.fontSize = 18;
        secondProgrammer.text = "youdontknowwhohackyou - человек с длинным никнеймом";
        thirdProgrammer.fontSize = 18;
        thirdProgrammer.text = "volodimir01 - верит, что фантазия существует у каждого";
        soundDesigner.fontSize = 18;
        soundDesigner.text = "Sa.ahas - Его называют Саней, но его погоняло Александер";
        softwareTestersLabel.fontSize = 18;
        softwareTestersLabel.text = "Yare_yare_yarek - Тоже давал ценные советы, на которые я сначала забил :). А потом он как-то потерялся..";
        firstSoftwareTester.text = "";
        secondSoftwareTester.text = "";
        thirdSoftwareTester.text = "";
    }
    public void OnBackToMainMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
