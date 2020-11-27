import React from 'react';

import Message from './Message';

const QueueWindow = (props) => {
    const messages = props.messages
        .map(m => <Message
            key={Date.now() * Math.random()}
            message={m.text} />);

    return (
        <div>
            <h4>Messages Queue:</h4>
            <br/>
            {messages.length > 0 && messages }
            {messages.length == 0 && (
                <h6>Sem mensagens</h6>
                )}
        </div>
    )
};

export default QueueWindow;