import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom"; // to create link in const Record () =>...

// one React component for entire table (Records)
// another react component for each row of the result set (Record)

// this info about record.name etc comes from backend in record.js
const Record = (props) => (
    <tr>
        <td>{props.record.name}</td>
        <td>{props.record.position}</td>
        <td>{props.record.level}</td>
        <td><Link to={`/edit/${props.record._id}`}>Edit</Link></td>

    </tr >
);
export default function Records() {
    const [records, setRecords] = useState([]); // pull data from backend, records will hold all data, initialized to empty array

    useEffect(() => {
        // put code to retrieve results, must work with backend
        // will trigger and render again when something is changed that's listed in the array at the end
        async function getRecords() {
            // request the backend
            const response = await fetch(`http://localhost:4000/record`); // connect to backend here
            if (!response.ok) {
                // get backend's json response
                const message = `An error occurred: ${response.statusText}`;
                window.alert(message);
                return;
            }
            const responseRecords = await response.json(); // wait for results to come back
            // put json list of records into records state
            setRecords(responseRecords); // set state
            return;
        }
        getRecords();
        return;

    }, [records.length] // retrigger use effect if you manually add records
    );

    function recordList() {
        // for each item in records, make a new record component
        return records.map((record) => { // is like a foreach loop
            return (
                // pass this information into the Record component
                <Record
                    record={record}
                    key={record._id}
                />
            );
        });
    }

    // HTML part to return table
    return (
        <div>
            <h3>Record List</h3>
            <table style={{ marginTop: 20 }}>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Position</th>
                        <th>Level</th>
                        <th>Edit</th>
                    </tr>
                </thead>
                <tbody>
                    {recordList()}
                </tbody>
            </table>
        </div>
    );
}
