import React from 'react';
import {StyleSheet, Text, View,Platform} from 'react-native';

import ITSWeb from "./src/ITSWeb"
import './config/sentry'
import { Constants, Location, Permissions } from 'expo';

export default class App extends React.Component {
    constructor(props){
        super(props);
    }
    render() {
        return (
            <View style={styles.container}>
                <ITSWeb/>
            </View>
        );
    }
    componentWillMount() {
        // if (Platform.OS === 'android' && !Constants.isDevice) {
        //     this.setState({
        //         errorMessage: 'Oops, this will not work on Sketch in an Android emulator. Try it on your device!',
        //     });
        // } else {
        //     this._getLocationAsync();
        // }
    }

    _getLocationAsync = async () => {
        let { status } = await Permissions.askAsync(Permissions.LOCATION);
        if (status !== 'granted') {
            this.setState({
                errorMessage: 'Permission to access location was denied',
            });
            this.timer = setInterval(()=> this._updateLocation(), 1000)
        }
    };

    _updateLocation = async () => {
        let location = await Location.getCurrentPositionAsync({});
        this.setState({ location });
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
});
