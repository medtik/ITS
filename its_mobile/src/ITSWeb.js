import React from "react" ;
import {View, StyleSheet, Dimensions, BackHandler, Platform} from "react-native" ;
import {WebView} from 'react-native-webview-messaging/WebView';
import notification from '../config/SetupNotification';
import { Constants, Location, Permissions, Notifications } from 'expo';
import Sentry from 'sentry-expo';

const ref = {
    webview: "REF_WEBVIEW"
};
export default class ITSWeb extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            uri: 'https://its-g8.firebaseapp.com/',
            canGoBack: false,
            websiteReady: false,
            pendingNotification: undefined
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

        if (Platform.OS === 'android' && !Constants.isDevice) {
            this.setState({
                errorMessage: 'Oops, this will not work on Sketch in an Android emulator. Try it on your device!',
            });
        } else {
            this._getLocationAsync();
        }
    }

    componentWillUnmount() {
        BackHandler.removeEventListener('hardwareBackPress', this.onHardwareBack);
    }

    _getLocationAsync = async () => {
        let { status } = await Permissions.askAsync(Permissions.LOCATION);
        if (status !== 'granted') {
            this.setState({
                errorMessage: 'Permission to access location was denied',
            });
            this.timer = setInterval(()=> this._updateLocation(), 60000)
        }
    };

    _updateLocation = async () => {
        let location = await Location.getCurrentPositionAsync({});
        console.debug('_updateLocation',location);
    };

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
            data,
            origin
        } = notification;

        Sentry.captureBreadcrumb({
            category: 'ITS-mobile',
            message: 'handleNotification',
            data: {
                notification,
                type: typeof notification,
                data,
                websiteReady: this.state.websiteReady
            }
        });

        if (origin != 'received') {
            if (this.state.websiteReady) {
                this.refs[ref.webview].sendJSON(data);
            } else {
                this.setState({
                    pendingNotification: notification
                })
            }
        }

        Sentry.captureMessage('handleNotification', {
            level: 'info'
        });
    }

    onWebsiteReady() {
        Sentry.captureBreadcrumb({
            category: 'ITS-mobile',
            message: 'onWebsiteReady'
        });
        this.setState({
            websiteReady: true
        });
        if(!!this.state.pendingNotification){
            this.handleNotification(notification);
        }

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
