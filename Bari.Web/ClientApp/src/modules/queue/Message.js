import React from 'react';

const Message = (props) => (
    <div style={{ background: "#eee", borderRadius: '5px', padding: '0 10px' }}>
        <pre>{JSON.stringify(JSON.parse(props.message), null, '\t')}</pre>
    </div>
);

export default Message;