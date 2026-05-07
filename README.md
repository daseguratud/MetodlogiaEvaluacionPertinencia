# Metodología para la Evaluación de Pertinencia Temática en Syllabus Mediante PLN

Este repositorio contiene los archivos, evidencias, software propietario y resultados asociados al desarrollo de la metodología propuesta para la evaluación de pertinencia temática en syllabus académicos mediante técnicas de Procesamiento de Lenguaje Natural (PLN), visualización cruzada de términos e indicadores cuantitativos de alineación temática.

El caso de estudio desarrollado se enfoca en espacios académicos relacionados con **Fundamentos de Sistemas Digitales**, empleando corpus de referencia obtenidos a partir de contenidos actuales del dominio.

***

## Objetivo

Desarrollar y validar una metodología sistemática que permita analizar el nivel de cobertura y alineación temática entre los contenidos de un syllabus y un conjunto de contenidos de referencia mediante:

* Procesamiento lingüístico de textos.

* Extracción de términos relevantes.

* Visualización cruzada de términos.

* Índices cuantitativos de cobertura y alineación temática.

***

## Descripción de directorios

* Etapa 00\_SyllabusIngenieriaElectrónica

  Planes de estudio recopilados desde las páginas institucionales de las universidades entregadas por el sistema de búsqueda del SNIES con los criterios de búsqueda seleccionados en el estudio.

* Etapa\_01

  Syllabus de los espacios académicos relativos a Fundamentos de Sistemas Digitales recopilados de las páginas oficiales de algunos de los programas de Ingeniería de la Universidad Distrital.

* Etapa\_02

  Contenidos de los syllabus recopilados en formato TXT obtenidos por medio de una herramienta OCR consolidados en el archivo "*rawTargetCorpus.txt*" por medio de la aplicación desarrollada para el experimento.

* Etapa\_03

  Contenido de los índices extraídos de la literatura de referencia empleada para el estudio en formato TXT y consolidados en el archivo "*rawReferenceCorpus.txt*"  por medio de la aplicación desarrollada para el experimento.

* Etapa\_04

  En esta carpeta se encuentran los archivos de reglas (*\_normalizationRules.txt*) y stopwords (*\_stopwords.csv*) empleados por la aplicación desarrollada para el experimento, los cuales son aplicados a los corpus obtenidos en las etapas dos y tres. El resultado del procesamiento realizado por la aplicación se encuentra en los archivos "*targetCorpus.csv*" y "*referenceCorpus.csv*", donde se registran los términos del corpus con sus correspondientes ocurrencias y frecuencia relativa. Adicionalmente, la aplicación genera las nubes de palabras correspondientes a cada corpus registrándolas en los archivos "*targetCorpus.png*" y "*referenceCorpus.png*".

* Etapa\_05

  Se presenta la información obtenida por medio de la aplicación desarrollada para el experimento registrada en dos archivos:

  * "*crossTerms.csv*", donde registra las frecuencias relativas de los términos relevantes para la comparación.

  * "*crossTerms.png*", donde se registra la visualización cruzada de términos resultante del procesamiento de los corpus.

* Experimento

  Solución desarrollada que automatiza el proceso de análisis en sus etapas dos a cinco, con ala cual se pueden replicar los resultados obtenidos en el experimento y realizar cambios en el mismo para verificar la validez del trabajo realizado. Es una solución de software basada en una aplicación de consola de C#.Net.

<br />

## Tecnologías utilizadas

* .NET

* C#

* GoogleTranslateLib - <https://github.com/TiepHoangDev/GoogleTranslateLib>

* WinForms Data Visualization - <https://github.com/kirsan31/winforms-datavisualization>

* WordCloudSharp - <https://github.com/AmmRage/WordCloudSharp>

***

## Índices propuestos

### Índice de Cobertura Temática (ICT)

Permite cuantificar el nivel de cobertura de los temas relevantes presentes en el corpus de referencia respecto al syllabus evaluado.

### Índice de Alineación Temática (IAT)

Permite cuantificar el nivel de alineación temática considerando la distribución de términos dentro de las regiones propuestas en la metodología.

***

## Resultados esperados

La metodología permite:

* Identificar temas alineados entre syllabus y contenidos de referencia.

* Detectar posibles vacíos temáticos.

* Analizar diferencias en el nivel de énfasis de determinados conceptos.

* Apoyar procesos de actualización curricular mediante métricas cuantitativas y visualización de resultados.

***

## Autores

Dario Alejandro Segura Torres - <dasegurat@udistrital.edu.co>

Jose-Luis Cabra López - <jlcabral@udistrital.edu.co>

Carlos Andrés Torres Pinzón - <caatorresp@udistrital.edu.co>

**Universidad Distrital Francisco José de Caldas**
