using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/* Содержит следующие элементы UI:
 * Slider, отвечающий за громкость музыки
 * Slider, отвечающий за громкость звуков
 * Toggle, отвечающий за полноэкранный режим. 
 * Button ResumeGame       | Обработчик нажатия кнопки: OnResumeGameButtonClick()
 * Button BackToMainMenu   | Обработчик нажатия кнопки: OnBackToMainMenuButtonClick()
 * Button ExitGame         | Обработчки нажатия кнопки: OnExitGameButtonClick()
 */
/// <summary>
/// Контроллирует функциональность внутриигрового меню настроек.
/// </summary>
public class MenuPanelLogic : MonoBehaviour
{
    /// <summary>
    /// Слайдер, контроллирующий громкость музыки.
    /// </summary>
    Slider musicVolumeSlider;

    /// <summary>
    /// Слайдер, контроллирующий громкость звуков.
    /// </summary>
    Slider soundVolumeSlider;

    /// <summary>
    /// Флаг полноэкранного режима.
    /// </summary>
    Toggle fullscreenToggle;

    /// <summary>
    /// Задний фон меню.
    /// </summary>
    GameObject menuBackground;

    /// <summary>
    /// Флаг, показывающий, активно ли внутриигровое меню
    /// </summary>
    private static bool IsMenuPanelActive;

    /// <summary>
    /// Делает внутриигровое меню активным.
    /// </summary>
    public void SetMenuActiveOn()
    {
        IsMenuPanelActive = true;
        menuBackground.SetActive(true);
    }

    /// <summary>
    /// Делает внутриигровое меню неактивным.
    /// </summary>
    public void SetMenuActiveOff()
    {
        IsMenuPanelActive = false;
        menuBackground.SetActive(false);
    }

    /// <summary>
    /// Проверяет, активно ли в данный момент внутриигровое меню настроек.
    /// </summary>
    /// <returns>Если активно, возвращает true. Иначе - false.</returns>
    public static bool IsMenuActive()
    {
        return IsMenuPanelActive;
    }

    private void Awake()
    {
        /* Получаем следующие графические элементы внутриигрового меню
         * Slider громкости музыки
         * Slider громкости звуков
         * Toggle полноэкранного режима
         * Panel задний фон внутриигрового меню
         */
        musicVolumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        soundVolumeSlider = GameObject.Find("SoundVolumeSlider").GetComponent<Slider>();
        fullscreenToggle = GameObject.Find("FullscreenToggle").GetComponent<Toggle>();
        menuBackground = GameObject.Find("MenuBackground");

        LoadSettings();     // Получаем пользовательские настройки предыдущей(если была) сессии
        SetMenuActiveOff(); // Изначально внутриигровое меню неактивно.
    }
    
    /// <summary>
    /// Загружает пользовательские настройки.
    /// </summary>
    private void LoadSettings()
    {
        // Если пользовательские настройки были сохранены в предыдущей игрой сессии
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            // Получаем значение громкости музыки и отрисовываем его на экране
            musicVolumeSlider.value = Convert.ToSingle(PlayerPrefs.GetInt("musicVolume"));
        }
        if(PlayerPrefs.HasKey("soundVolume"))
        {
            // Получаем значения громкости звуков и отрисовываем его на экране
            soundVolumeSlider.value = Convert.ToSingle(PlayerPrefs.GetInt("soundVolume"));
        }
    }
    /// <summary>
    /// Сохраняет пользовательские настройки.
    /// </summary>
    private void SaveSettings()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("musicVolume", Convert.ToInt32(musicVolumeSlider.value));
        PlayerPrefs.SetInt("soundVolume", Convert.ToInt32(soundVolumeSlider.value));
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Вызывается, когда пользователем нажата кнопка Resume game.
    /// </summary>
    public void OnResumeGameButtonClick()
    {
        SaveSettings();
    }
    /// <summary>
    /// Вызывается, когда пользователем нажата кнопка Exit game.
    /// </summary>
    public void OnExitGameButtonClick()
    {
        Application.Quit();
    }
}
