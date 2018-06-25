import React from 'react';
import {StyleSheet, Text, View} from 'react-native';
import ITSWeb from "./src/ITSWeb"

export default class App extends React.Component {
    render() {
        return (
            <View style={styles.container}>
                <ITSWeb/>
            </View>
        );
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
