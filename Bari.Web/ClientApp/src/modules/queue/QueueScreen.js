import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

import QueueWindow from './QueueWindow';

const QueueScreen = () => {
    const [messages, setMessages] = useState([]);
    const latestMessages = useRef(null);

    latestMessages.current = messages;

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/hubs/queue')
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(result => {
                console.log('Connected!');

                connection.on('SendMessageOutPut', message => {
                    const updatedMessages = [...latestMessages.current];
                    updatedMessages.push(message);

                    setMessages(updatedMessages);
                });
            })
            .catch(e => console.log('Connection failed: ', e));
    }, []);

   
    return (
        <div>
            <QueueWindow messages={messages} />
        </div>
    );
};

export default QueueScreen