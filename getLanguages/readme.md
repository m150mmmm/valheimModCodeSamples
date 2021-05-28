ローカライズ用のjsonを用意していない言語の場合、変数名が表示されてしまう問題があるため用意したファイル  

使い方  
  
getLanguages.dll  
通常のmodと同じように**\SteamApps\common\Valheim\BepInEx\pluginsに置いて使う
セッティングの言語選択の次へ(右の矢印)押下時にコンソールに対応言語の一覧を表示する  
前提mod: BepInEx  
   
copyLocalization.bat  
**\SteamApps\common\Valheim\BepInEx\plugins\【MOD名】\Assets\Translationsに置いて使う  
\Assets\Translations\Englishを全対応言語用にコピーする  
日本語は自分で用意するとかそういう場合はJapaneseの行を削除して使用してください  
  
それと言語名フォルダ以下に置くjsonファイル自体の名前は任意の名前でいいようです  
  
下記urlを見た感じ、翻訳ファイルが無い場合のデフォルトでenglishを参照とかそういう機能はなさそう  
参考文献  
https://valheim-modding.github.io/Jotunn/tutorials/localization.html  
