# Browser UO Assets

Place browser-ready UO asset sets here.

Recommended layout:

`browser-assets/uo/versions/<asset-version>/...`

Set the active version in:

`browser-assets/uo/active-version.txt`

The browser publish script copies the selected version into the web bundle and generates `uo-manifest.json` so the browser runtime can preload the files into its virtual `/uo` filesystem.
