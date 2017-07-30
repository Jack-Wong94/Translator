# Translator
Take a photo and translate it from English to Chinese

## Features:
* Using Xamarin.Form to create a translator app that translate English into Chinese. The purpose of this app is to help them understand English. I.e. If someone see a sign and dont know what it means, they could just snap a photo and the app would translate the text on the photo to Chinese. 

* 3 different apis are used to construct this app.
1. Microsoft cognitive service computer vision api: This api takes a picture as input and return any texts on the picture. For now it can only recognise printed text but not hand written text but it could be configured to recognise handwritten text.
2. Yandex Translator api: This api takes the an input text and returns a translated text based on the lanugage code. In this app, the language code is en-zh (English to Chinese). Further development would allows user to switch the language code.
3. TextGain api: This api analysis the word type of each word in the sentence.

* Using github for source control. Using different branches to test out different features.
1. CustomVision branch: Test out the microsoft custom feature api.
2. FacebookLoginLogOut: Test the authentication feature provided on Microsoft Azure.
* Host the app on azure platform and utilise the easy table to allow user update the easy table and retrieve learned vocab from the easy table.

## Future development:
* Polish the interface.
* Provide support to multiple languages.
* Provide authentication to the user via Facebook.