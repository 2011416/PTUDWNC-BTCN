import React, { useEffect } from 'react';

const RSS = () => {
    useEffect(() => {
        document.title = 'RSS';
    }, []);

    return (
        <h1>
        Đây là trang RSS
        </h1>
    );
}

export default RSS;