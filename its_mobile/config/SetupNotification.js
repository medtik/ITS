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

async function handleNotifications(notification) {
    let token = await registerForPushNotificationsAsync();

    const notificationBody = {
        to: token,
        title: "some title here",
        body: "body body boy"
    };

    notification.to = token;

    console.log(notification);
    return fetch("https://exp.host/--/api/v2/push/send", {
        method: 'POST',
        headers: new Headers({
            'Accept': 'application/json',
            'Accept-Encoding': 'gzip, deflate',
            'Content-Type': 'application/json',
        }),
        body: JSON.stringify(notification)
    })
        .then((response) => response.text())

}

export default {
    handleNotifications,
    registerForPushNotificationsAsync
}