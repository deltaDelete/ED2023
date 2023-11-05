using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace ED2023.App.Utils; 

public class MessageBoxUtils {
    /// <summary>
    /// Создает диалог с текстом и кнопками Да/Нет, при использовании результата положительный сравнивать с "Да"
    /// </summary>
    /// <param name="title">Заголовок диалога</param>
    /// <param name="message">Сообщение диалога</param>
    public static IMsBox<string> CreateConfirmMessageBox(string title, string message) {
        return MessageBoxManager.GetMessageBoxCustom(
            new() {
                ButtonDefinitions = new ButtonDefinition[] {
                    new() { Name = "Да", IsDefault = true },
                    new() { Name = "Нет", IsCancel = true }
                },
                ContentTitle = title,
                ContentMessage = message,
                Icon = Icon.None,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = true,
            }
        );
    }
}