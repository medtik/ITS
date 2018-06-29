import {Permissions, Notifications} from 'expo';
import React from 'react'

async function registerForPushNotificationsAsync() {
    const {status: existingStatus} = await Permissions.getAsync(
        Permissions.NOTIFICATIONS
    );
    let finalStatus = existingStatus;

    // only ask if permissions have not already been determined, because
    // iOS won't necessarily prompt the user a second time.
    if (existingStatus !== 'granted') {
        // Android remote notification permissions are granted during the app
        // install, so this will only ask on iOS
        const {status} = await Permissions.askAsync(Permissions.NOTIFICATIONS);
        finalStatus = status;
    }

    // Stop here if the user did not grant permissions
    if (finalStatus !== 'granted') {
        return;
    }

    // Get the token that uniquely identifies this device
    return await Notifications.getExpoPushTokenAsync();
}

async function handleNotifications(notifications) {
    let token = registerForPushNotificationsAsync();

    const notificationBody = {
        to: token,
        title: "some title here",
        body: "body body boy"
    };

    fetch("https://exp.host/--/api/v2/push/send", {
        method: 'POST',
        headers: new Headers({
            'Accept': 'application/json', // <-- Specifying the Content-Type
            'Accept-Encoding': 'gzip, deflate', // <-- Specifying the Content-Type
            'Content-Type': 'application/json', // <-- Specifying the Content-Type
        }),
        body: JSON.stringify(notificationBody) // <-- Post parameters
    })
        .then((response) => response.text())
        .then((responseText) => {
            alert(responseText);
        })
        .catch((error) => {
            console.error(error);
        });
}

export default handleNotifications