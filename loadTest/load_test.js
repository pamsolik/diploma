import http from "k6/http";

export default function() {
    const options = {
        headers: {
            "Authorization": `Bearer CfDJ8FvwE6TjOvFFuw1z9CKtT7KVDWn-qVfFifYP1Uo_JLmB14_6QOTOiduRrKTuQzfDvh5ruHr8YLODGCwOO-vzmEszbZmg2J1ill04ExVGFfYYe0CgoyRoEsZewXY27cq7MBS9O_CC2j6qA63dH90JWEc`,
            "accept": 'application/json',
            "Content-Type": 'application/json-patch+json'
        },
    };

    const b = JSON.stringify({
        pageSize: 5,
        pageIndex: 0
      })
    const url = "https://pamsolik-cars.azurewebsites.net/api/recruitments/public"
    //const url = "https://localhost:7187/api/recruitments/public"
    let response = http.post(url, b, options);
    //console.log(response.body)
};