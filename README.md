</br>
</br>
<p align="center">
<img width="400" src="https://github.com/user-attachments/assets/454f39f7-ef37-4102-806d-031db29bc4f5">
</p>
</br>
</br>

<p align="center">
<img alt="Version" src="https://img.shields.io/github/package-json/v/DCFApixels/VectorFields?style=for-the-badge&color=1e90ff">
<img alt="License" src="https://img.shields.io/github/license/DCFApixels/VectorFields?color=1e90ff&style=for-the-badge">
</p>

# VectorFields

**VectorFields** добавляет в Unity новые атрибуты для кастомизации отображения полей в инспекторе:
* `[VectorField]` - Предназначен для отрисовки всех полей векторных типов в одну строчку, аналогично тому как отображаются типы вроде Vector3.
* `[EulerField]` - Отрисовывает векторный тип как кватернион конвертированный в углы Эйлера.
* `[ColorField]` - Отрисовывает векторный тип в виде поля с выбором цвета.
* `[Color32Field]` - Аналогично ColorField, но для 8 битного представления цвета.
* `[ColorHSVField]` - Аналогично ColorField, но для представления цвета в формате HSV.

> Атрибуты для отрисовки цветов работают совместно с атрибутом `[ColorUsage]`, но только если `[ColorUsage]` идет после `[ColorField]` или его аналогов.

<p align="center">
<img src="https://github.com/user-attachments/assets/3aacc2d0-a6ef-4da6-9953-00605fd7499b" width="600" >
<br>

</p>

## Установка
Семантика версионирования - [Открыть](https://gist.github.com/DCFApixels/e53281d4628b19fe5278f3e77a7da9e8#file-dcfapixels_versioning_ru-md)

* ### Unity-модуль
Добавьте git-URL в [PackageManager](https://docs.unity3d.com/2023.2/Documentation/Manual/upm-ui-giturl.html) или вручную в `Packages/manifest.json` файл. Используйте этот git-URL: 
```
https://github.com/DCFApixels/VectorFields.git
```
* ### В виде исходников
Можно установит просто скопировав исходники в папку проекта
