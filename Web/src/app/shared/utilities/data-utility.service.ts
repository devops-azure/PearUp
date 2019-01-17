export class DataUtilityService {
    private static instance: DataUtilityService;

    static getInstance() {
        return DataUtilityService.instance == null ?
            DataUtilityService.instance = new DataUtilityService() :
            DataUtilityService.instance;
    }

    get(key) {
        return this[key];
    }

    set(key, value) {
        this[key] = value;
    }

    private constructor(copy?) {
        for (let key in copy || []) {
            this.set(key, copy[key]);
        }
    }
}
