import { useState } from 'react';
import { GetPharmaciesToVisitRequest } from '../api-client';

const PharmacyVisit = () => {
    const [file, setFile] = useState<File>();
    const [fileName, setFileName] = useState<string>();
    const [x, setX] = useState(0);
    const [pharmacyNamesToVisit, setPharmacyNamesToVisit] = useState<string[]>([]);

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            setFile(e.target.files[0]);
            setFileName(e.target.files[0].name);
        }
    };

    const handleFileUpload = async () => {
        if (file && fileName) {
            const formData = new FormData();

            formData.append("file", file);

            try {
                await fetch(`${window.location.protocol + "//" + window.location.hostname + (process.env.REACT_APP_USE_PROXY === 'true' ? '/api' : ':5000')}/pharmacyvisit/insert`, {
                    method: 'POST',
                    body: formData
                });
            }
            catch (ex) {

            }
        }
    };

    const handleGetPharmacies = async () => {
        try {
            const response = await fetch(`${window.location.protocol + "//" + window.location.hostname + (process.env.REACT_APP_USE_PROXY === 'true' ? '/api' : ':5000')}/pharmacyvisit`, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ x } as GetPharmaciesToVisitRequest)
            });

            if (response.status === 200) {
                const data = await response.json();

                setPharmacyNamesToVisit(data.result.pharmacyNames);
            }
        }
        catch (ex) {

        }
    };

    return (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '4px', padding: '7px' }}>
            To upload new data, choose a single file then click Upload. Data is wiped clean before new data is inserted.
            <div style={{ display: 'flex', gap: '4px' }}>
                <input type='file' onChange={handleFileChange} />
                <input type='button' value='Upload' onClick={handleFileUpload} />
            </div>
            <div style={{ display: 'flex', gap: '4px' }}>
                <input type='button' value='Get' onClick={handleGetPharmacies} />
                <input type='number' value={x} step='1' min={0} onChange={(e) => setX(parseInt(e.target.value))} />
                <span>Pharmacies</span>
            </div>
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '4px' }}>
                {
                    pharmacyNamesToVisit.length > 0 && pharmacyNamesToVisit.map((x, i) =>
                        <div key={`${x}_${i}`} style={{ display: 'flex', border: '1px solid black', padding: '4px' }}>{x}</div>
                    )
                }
            </div>
        </div >
    );
};

export default PharmacyVisit;