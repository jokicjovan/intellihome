import axios from 'axios';
import {environment} from "../../security/Environment.tsx";

const WashingMachineService = {
    GetWashingMachineModes: async () => {
        axios.get(
            `${environment}/api/PKA/GetWashingMachineModes`,
        )
            .then((res) => {
                if (res.status === 200){
                    const modes : ModeCheckbox[] = []
                    res.data.forEach((mode: WashingMachineMode) => {
                        modes.push({ display: `${mode.name} (${mode.duration} min) (${mode.temperature}°C)`, value: mode.id })
                    });
                    setModesCheckboxes(modes);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    },
};

export default WashingMachineService;