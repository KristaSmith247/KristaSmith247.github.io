import React, { useEffect, useState } from "react";

// One React component for the entire table (Records)
// Another React component for each row of the result set (Record)


const HelloWorld = (props) => (
    <div>{props.record.teamName}</div>
);


export default function RecordList() {
    const [records, setRecords] = useState([]);


    useEffect(() => {
        async function getRecords() {
            // connect to address in config.env file
            const response = await fetch(`http://localhost:4000/record/`);


            if (!response.ok) {
                const message = `An error occurred: ${response.statusText}`;
                window.alert(message);
                return;
            }
            const records = await response.json();
            setRecords(records);
        }

        getRecords();
        return;
    });

    function recordList() {
        return records.map((record) => {
            return (
                <HelloWorld record={record}
                    key={record._id}
                />

            )
        })
    }



    return (
        <div>
            <h1>Hello World!!</h1>
            <div>
                {recordList()}
            </div>
        </div>
    )
}
