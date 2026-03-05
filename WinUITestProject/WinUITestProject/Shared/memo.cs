using System.Collections.Generic;


// プロパティ変更通知の仕組みを使うための名前空間
using System.ComponentModel;

// CallerMemberName 属性を使うための名前空間
using System.Runtime.CompilerServices;

// すべての ViewModel の親クラス
public class ViewModelBase : INotifyPropertyChanged
{
    // 「プロパティが変更されたよ」というイベント
    // View (XAML) 側はこれを監視して画面を更新する
    public event PropertyChangedEventHandler? PropertyChanged;

    // プロパティ変更を通知するためのメソッド
    // name は変更されたプロパティ名
    protected void OnPropertyChanged(
        // CallerMemberName により、
        // このメソッドを呼び出したプロパティ名が自動で入る
        [CallerMemberName] string? name = null)
        // PropertyChanged イベントを発火させる
        => PropertyChanged?.Invoke(
            this,                             // 変更元（この ViewModel）
            new PropertyChangedEventArgs(name) // どのプロパティが変わったか
        );

    // プロパティの値を安全にセットするための汎用メソッド
    protected bool SetProperty<T>(
        ref T field,                        // 実体となる private フィールド
        T value,                            // 新しく設定したい値
        [CallerMemberName] string? name = null // 呼び出し元のプロパティ名
    )
    {
        // 今の値と新しい値が同じなら何もしない
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        // 値が違えばフィールドを更新
        field = value;

        // プロパティ変更通知を発火
        OnPropertyChanged(name);

        // 実際に変更が行われたことを返す
        return true;
    }
}