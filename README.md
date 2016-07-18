# Bucketed App Notes

Application development completed in Xamarin Studio on OSX, tested in iOS Simulator.
Screenshots provided of application interface.

## Development Hurdles

1. Progress on the application was significantly delayed with issues around configuring the Parse package from NuGet to integrate into the Xamarin application. The project was set back by almost two weeks before these issues were resolved and reliable communication with the Parse platform were established.
2. With the delay in project timeline caused by Parse integration, project development and testing was focused on the iOS platform rather than divided between the two platforms with the goal of getting one application functioning correctly.
3. After the project suffered such a significant setback, the GPS integration was dropped to focus on completing the other core tasks.
4. When switching to Android development it was discovered that one of the local classnames "Activity" was a reserved classname in the Xamarin.Android platform. The project was refactored to rename the Activity class to Attraction, along with changing references to similarly named functions. E.g. "GetActivitesAsync". The Android version is now feature complete, but would still require additional UI refinement.
5. When the Parse object was renamed, the data collection started again with the name "Attraction", losing the previously entered example data, as the Parse platform did not offer a function to rename an existing collection.

## Application Features
1. Public "Discover" list of activities
2. Combined Register & Login page (Form changes depending if email has is registered)
3. App remembers login (using Parse CurrentUser)
4. Logged in users can add activities to their bucket list
5. Activities added to the bucket list can be "Completed" which moves them the Achieved list
6. User can create a new activity
7. Creating an activity includes adding an image from Camera (or Photo Library if camera is unavailable)
8. User's image is resized and saved to Parse server as a ParseFile object
9. Resized user image is referenced by ParseFile url
10. Created activity is added to Discover list where it can be "Bucketed" by users

## Nuget Packages installed were
* Parse (for remote data storage)
* Xam.Plugin.Media (for camera integration)

## Other references
* Xamarin Forums
* Image Resize - https://forums.xamarin.com/discussion/37681/how-to-resize-an-image-in-xamarin-forms-ios-android-and-wp#latest
