using System;
using UnityEngine;
using UnityEngine.UI;

/* Содержит следующие UI элементы:
 * Slider, отвечающий за громкость музыки       | Обработчик изменения значения: OnMusicSliderValueChange()
 * Slider, отвечающий за громкость звуков       | Обработчик изменения значения: OnSoundSliderValueChange()
 * Text, отображающая значение громкости музыки
 * Text, Отображающая значение громкости звуков
 * Button BackToMain    | 
 * Button SaveChanges   | Обработчик нажатия кнопки: OnSaveChangesButtonClick()
 */
/// <summary>
/// Контроллирует функциональность меню настроек.
/// </summary>
public class SettingsMenuLogic : MonoBehaviour
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
    /// Метка, отображающая значение громкости музыки.
    /// </summary>
    Text musicVolumeValueLabel;

    /// <summary>
    /// Метка, отображающая значение громкости звуков.
    /// </summary>
    Text soundVolumeValueLabel;

    /// <summary>
    /// TODO
    /// </summary>
    Toggle fullscreenToggle;

    /// <summary>
    /// Кнопка сохранения изменений настроек.
    /// </summary>
    GameObject saveChangesButton;
    private void Awake()
    {
        /* Получаем следующие элементы меню настроек:
         * Slider громкости музыки
         * Text для отображения значения громкости музыки
         * Slider громкости звуков
         * Text для отображения значения громкости звуков
         * Button сохранения изменений настроек
         */
        musicVolumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        musicVolumeValueLabel = GameObject.Find("MusicVolumeValueLabel").GetComponent<Text>();
        soundVolumeSlider = GameObject.Find("SoundVolumeSlider").GetComponent<Slider>();
        soundVolumeValueLabel = GameObject.Find("SoundVolumeValueLabel").GetComponent<Text>();
        saveChangesButton = GameObject.Find("SaveChangesButton");

        LoadSettings();                     // Получаем настройки предыдущей(если была) игровой сессии
        saveChangesButton.SetActive(false); // Изначально нет никаких изменений настроек, поэтому кнопка неактивна
    }

    /// <summary>
    /// Вызывается, когда пользователь изменяет громкость музыки
    /// </summary>
    public void OnMusicSliderValueChange()
    {
        // Отображаем в метке значение, соответствующее положению бегунка musicVolumeSlider
        musicVolumeValueLabel.text = Convert.ToInt32(musicVolumeSlider.value).ToString();
        // Отображаем кнопку сохранения изменений.
        saveChangesButton.SetActive(true);
    }

    /// <summary>
    /// Вызывается, когда пользователь изменяет громкость звуков.
    /// </summary>
    public void OnSoundSliderValueChange()
    {
        // Отображаем в метке значение, соответствующее положению бегунка soundVolumeSlider
        soundVolumeValueLabel.text = Convert.ToInt32(soundVolumeSlider.value).ToString();
        // Отображаем кнопку сохранения изменений.
        saveChangesButton.SetActive(true);
    }

    //TODO
    public void OnFullscreenToogleChange()
    {

    }

    /// <summary>
    /// Вызывается при нажатии пользователем кнопки сохранения изменений.
    /// </summary>
    public void OnSaveChangesButtonClick()
    {
        SaveSettings();
        // Скрываем кнопку сохранения настроек.
        saveChangesButton.SetActive(false);
    }

    /// <summary>
    /// Сохраняет текущие настроки пользователя.
    /// </summary>
    private void SaveSettings()
    {
        /* Сохраняем настройки пользователя. 
         * Ключи сохранения:
         * musicVolume - целочисленное значение громкости музыки
         * soundVolume - целочисленное значение громкости звуков
         */
        PlayerPrefs.DeleteAll();    // Удаляем все предыдущие пользовательские настройки
        PlayerPrefs.SetInt("musicVolume", Convert.ToInt32(musicVolumeSlider.value));
        PlayerPrefs.SetInt("soundVolume", Convert.ToInt32(soundVolumeSlider.value));
        PlayerPrefs.Save();
        Debug.Log("New settings was saved!");

    }

    /// <summary>
    /// Загружает пользовательские настройки.
    /// </summary>
    private void LoadSettings()
    {
        // Если существуют какие-то сохраненные данные, то мы их считываем
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            musicVolumeSlider.value = Convert.ToSingle(PlayerPrefs.GetInt("musicVolume"));
            musicVolumeValueLabel.text = musicVolumeSlider.value.ToString();
        }
        if(PlayerPrefs.HasKey("soundVolume"))
        {
            soundVolumeSlider.value = Convert.ToSingle(PlayerPrefs.GetInt("soundVolume"));
            soundVolumeValueLabel.text = soundVolumeSlider.value.ToString();
        }

    }

}
