﻿using MaterialDesignThemes.Wpf;
using Sheas_Cealer.Consts;
using Sheas_Cealer.Preses;
using Sheas_Cealer.Props;
using Sheas_Cealer.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sheas_Cealer.Wins;

public partial class SettingsWin : Window
{
    private readonly SettingsPres SettingsPres;

    internal SettingsWin()
    {
        InitializeComponent();

        DataContext = SettingsPres = new();
    }
    private void SettingsWin_SourceInitialized(object sender, EventArgs e)
    {
        IconRemover.RemoveIcon(this);
        BorderThemeSetter.SetBorderTheme(this, SettingsPres.IsLightTheme);
    }

    private void ThemesButton_Click(object sender, RoutedEventArgs e) => SettingsPres.IsLightTheme = SettingsPres.IsLightTheme.HasValue ? SettingsPres.IsLightTheme.Value ? null : true : false;
    private void LangsButton_Click(object sender, RoutedEventArgs e)
    {
        SettingsPres.IsEnglishLang = SettingsPres.IsEnglishLang.HasValue ? SettingsPres.IsEnglishLang.Value ? null : true : false;

        MessageBox.Show(SettingsConst._ChangeLangSuccessMsg);
    }
    private void ColorsButton_Click(object sender, RoutedEventArgs e)
    {
        Random random = new();
        PaletteHelper paletteHelper = new();
        Theme newTheme = paletteHelper.GetTheme();
        Color newPrimaryColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));

        newTheme.SetPrimaryColor(newPrimaryColor);
        paletteHelper.SetTheme(newTheme);

        Style newButtonStyle = new(typeof(Button), Application.Current.Resources[typeof(Button)] as Style);
        (Color? newForegroundColor, Color newAccentForegroundColor) = ForegroundGenerator.GetForeground(newPrimaryColor.R, newPrimaryColor.G, newPrimaryColor.B);

        newButtonStyle.Setters.Add(new Setter(ForegroundProperty, newForegroundColor.HasValue ? new SolidColorBrush(newForegroundColor.Value) : new DynamicResourceExtension("MaterialDesignBackground")));
        Application.Current.Resources[typeof(Button)] = newButtonStyle;

        SettingsPres.AccentForegroundColor = newAccentForegroundColor;

        Settings.Default.PrimaryColor = System.Drawing.Color.FromArgb(newPrimaryColor.A, newPrimaryColor.R, newPrimaryColor.G, newPrimaryColor.B);
        Settings.Default.Save();
    }
    private void WeightsButton_Click(object sender, RoutedEventArgs e) => SettingsPres.IsLightWeight = SettingsPres.IsLightWeight.HasValue ? SettingsPres.IsLightWeight.Value ? null : true : false;

    private void SettingsWin_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            Close();
    }
}