import React from "react" ;
import {View, StyleSheet, Dimensions, BackHandler} from "react-native" ;
import {WebView} from 'react-native-webview-messaging/WebView';
import notification from '../config/SetupNotification';
import {Notifications} from "expo";

import Sentry from "sentry-expo";

const ref = {
    webview: "REF_WEBVIEW"
};
export default class ITSWeb extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            // uri: 'http://its8.gear.host/',
            // uri: 'http://192.168.2.2:80/',
            uri: 'http://172.16.3.180:80/',
            canGoBack: false,
        };

        this.onLayout = this.onLayout.bind(this);
        this.onHardwareBack = this.onHardwareBack.bind(this);
        this.onNavigationStateChange = this.onNavigationStateChange.bind(this);
    }

    componentDidMount() {
        this.onLayout();

        BackHandler.addEventListener('hardwareBackPress', this.onHardwareBack);
        Notifications.addListener(this.handleNotification.bind(this));
        this.registerWebChannel();
    }

    componentWillUnmount() {
        BackHandler.removeEventListener('hardwareBackPress', this.onHardwareBack);
    }

    onLayout() {
        const dimension = Dimensions.get('window');
        this.setState({
            width: dimension.width,
            height: dimension.height
        });
    }

    registerWebChannel() {
        this.refs[ref.webview].messagesChannel.on('json', json => {
            Sentry.captureBreadcrumb({
                category: 'ITS-mobile',
                message: 'componentDidMount',
                data: {
                    message: json
                }
            });

            const {
                type,
                payload
            } = json;

            switch (type) {
                case 'ready':
                    this.onWebsiteReady();
                    break;
                default:
                    Sentry.captureException(new Error('Invalid message'));
            }
        });
    }

    handleNotification(notification) {
        const {
            data
        } = notification;

        Sentry.captureBreadcrumb({
            category: 'ITS-mobile',
            message: 'handleNotification',
            data: {
                notification,
                type: typeof notification,
                data
            }
        });

        this.refs[ref.webview].sendJSON(data);
        Sentry.captureMessage('handleNotification',{
            level: 'info'
        });
    }

    onWebsiteReady() {
        Sentry.captureBreadcrumb({
            category: 'ITS-mobile',
            message: 'onWebsiteReady'
        });
        notification.registerForPushNotificationsAsync()
            .then(token => {
                Sentry.captureBreadcrumb({
                    category: 'ITS-mobile',
                    message: 'registerForPushNotificationsAsync',
                    data: {
                        token
                    }
                });

                this.refs[ref.webview].sendJSON({
                    type: 'expToken',
                    payload: {
                        token
                    }
                })
            })
            .catch(ex => {
                Sentry.captureException(ex);
            })
    }

    onHardwareBack() {
        if (this.state.canGoBack) {
            this.refs[ref.webview].goBack();
        }
        return this.state.canGoBack;
    }

    onNavigationStateChange(ex) {
        this.setState({
            canGoBack: ex.canGoBack
        });
    }

    render() {
        const style = StyleSheet.create({
            container: {
                backgroundColor: "#007AFF",
                paddingTop: 23
            },
            webView: {
                width: this.state.width,
                height: this.state.height,
            }
        });

        return (
            <View style={style.container}
                  onLayout={this.onLayout}>
                <WebView
                    ref={ref.webview}
                    onNavigationStateChange={this.onNavigationStateChange}
                    source={{uri: this.state.uri}}
                    style={style.webView}
                    userAgent="its8_demo"
                />
            </View>
        );
    }
}
