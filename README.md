# Translator
Take a photo and translate it from English to Chinese

## Features:
1. Using Xamarin.Form to create a translator app that translate English into Chinese. The purpose of this app is to help them understand English. I.e. If someone see a sign and dont know what it means, they could just snap a photo and the app would translate the text on the photo to Chinese. 

2. 3 different apis are used to construct this app.
    * Microsoft cognitive service computer vision api: This api takes a picture as input and return any texts on the picture. For now it can only recognise printed text but not hand written text but it could be configured to recognise handwritten text.
    * Yandex Translator api: This api takes the an input text and returns a translated text based on the lanugage code. In this app, the language code is en-zh (English to Chinese). Further development would allows user to switch the language code.
    * TextGain api: This api analysis the word type of each word in the sentence.

3. Using github for source control. Using different branches to test out different features.
    * CustomVision branch: Test out the microsoft custom feature api.
    * FacebookLoginLogOut: Test the authentication feature provided on Microsoft Azure.

4. Host the app on azure platform and utilise the easy table to allow user update the easy table (Vocab Model) and retrieve learned vocab from the easy table.

## Future development:
* Polish the interface.
* Provide support to multiple languages.
* Provide authentication to the user via Facebook.