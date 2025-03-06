using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserProfile", menuName = "UserProfile")]
public class UserProfile : ScriptableObject
{
    [SerializeField] private ProfileData _profileData;
    public ProfileData ProfileData => _profileData;
    public void SetProfileData(ProfileData profileData)
    {
        _profileData = profileData;
    }
}
